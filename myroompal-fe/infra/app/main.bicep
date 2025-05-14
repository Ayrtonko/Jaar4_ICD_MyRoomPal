param sku string = 'Free'
param location string = 'westeurope'
param appName string = 'myroompal-fe'
param environment string = 'default'


module staticSite 'br/public:avm/res/web/static-site:0.6.1' = {
  name: 'staticSiteDeployment-${uniqueString(resourceGroup().id)}-${uniqueString(environment)}'
  params: {
    // Required parameters
    name:  '${appName}-${environment}'
    // Non-required parameters
    location: location
    sku: sku
    enableTelemetry: false
  }
}

