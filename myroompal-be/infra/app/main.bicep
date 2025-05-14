param webAppName string = 'myroompal' 
param environment string = 'tst'
param sku string = 'F1' // The SKU of App Service Plan
param location string = 'westeurope'
param linuxFxVersion string = 'DOTNETCORE|8.0'
var appServicePlanName = toLower('asp-${webAppName}-${environment}')
var webSiteName = toLower('${webAppName}-${environment}-api')

//db
param sqlDbName string = '${webAppName}-database-default'
param sqlServerName string = '${webAppName}-dbServer-default'
param dbAdministratorLogin string = 'MyRoomPalAdmin'
@secure()
param dbAdministratorLoginPassword string = newGuid()
param sqlAdminLoginKeyName string = 'sqlAdminLoginKeyName-default'
param sqlConnectionStringKeyName string = 'sqlConnectionStringKey-default'

resource existingKeyVault 'Microsoft.KeyVault/vaults@2022-07-01' existing = {
  name: 'myroompal-kv'
}

module server 'br/public:avm/res/sql/server:0.10.1' = {
  name: 'serverDeployment'
  params: {
    // Required parameters
    name: sqlServerName
    // Non-required parameters
    administratorLogin: dbAdministratorLogin
    administratorLoginPassword: dbAdministratorLoginPassword
    
    databases: [
      {
        name: sqlDbName
        sku:{
          name:'Basic'
          tier:'Basic'
        }
        zoneRedundant: false
        maxSizeBytes: 2147483648 
      }
    ]
    location: location
    secretsExportConfiguration: {
      keyVaultResourceId: existingKeyVault.id
      sqlAdminPasswordSecretName: sqlAdminLoginKeyName
      sqlAzureConnectionStringSercretName: sqlConnectionStringKeyName
    }
  }
}

module WorkspaceAnalytics 'br/public:avm/res/operational-insights/workspace:0.9.0' = {
  name: 'log-analytics-module'
  params: {
    // Required parameters
    name: 'wsa-${environment}'
    // Non-required parameters
    location: resourceGroup().location
  }
}

module ApplicationInsights 'br/public:avm/res/insights/component:0.4.2' = {
  name: 'app-insights-module'
  params: {
    // Required parameters
    name: 'appinsights-${environment}'
    workspaceResourceId: WorkspaceAnalytics.outputs.resourceId
    // Non-required parameters
    location: resourceGroup().location
  }
}


module AppServicePlan 'br/public:avm/res/web/serverfarm:0.3.0' = {
  name: 'appServicePlanDeployment'
  params: {
    // Required parameters
    name: appServicePlanName
    // Non-required parameters
    skuCapacity: 1
    location: location
    skuName: sku
    kind:'Linux'
    reserved:true
    enableTelemetry: false
  }
}

resource AppService 'Microsoft.Web/sites@2020-06-01' = {
  name: webSiteName
  location: location
  identity: {
      type: 'SystemAssigned'
    }
  properties: {
    serverFarmId: AppServicePlan.outputs.resourceId
    siteConfig: {
      linuxFxVersion: linuxFxVersion
      appSettings:[
        {
          name: 'APP_ENVIRONMENT'
          value: environment
        }
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: '${ApplicationInsights.outputs.connectionString}'
        }
      ]
    }
  }
}

resource connectionstrings 'Microsoft.Web/sites/config@2021-03-01' = { 
  name: 'connectionstrings'
  parent: AppService
  kind: 'string'
  properties: { 
    HubDbContext: { 
      value: '@Microsoft.KeyVault(VaultName=myroompal-kv;SecretName=sqlConnectionStringKey-${environment})'
      type: 'SQLAzure' 
    }
  }
}

resource keyVaultAccessPolicy 'Microsoft.KeyVault/vaults/accessPolicies@2022-07-01' = {
  parent: existingKeyVault
  name: 'add'
  properties: {
    accessPolicies: [
      {
        tenantId: subscription().tenantId
        objectId: AppService.identity.principalId
        permissions: {
          secrets: [
            'get'
            'list'
            'set'
            'delete'
          ]
        }
      }
    ]
  }
  dependsOn: [
    AppService
  ]
}
