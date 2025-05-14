// https://learn.microsoft.com/en-us/azure/app-service/provision-resource-bicep
param location string = 'westeurope'
var keyvault = 'myroompal-kv'
param sku string = 'standard'
param objectIds array = [
  '5d6e6c29-5ded-4d88-9871-7ab98457f4b7'
  '3f01441c-e4d9-44a8-874c-de1cf883ccb8' 
  '0361a525-8499-428e-9080-05f70eeda3bf' 
  '5c424c04-dcba-420b-9c5e-4989f2372fa2'
  '527d1802-1b85-4db9-b312-a89d14cdc364'
  '8fa6bc59-aaa9-4b58-9854-69dda607d65c'
]

module keyVault 'br/public:avm/res/key-vault/vault:0.10.2' = {
  name: 'vaultDeployment'
  params: {
    name: keyvault
    location: location
    sku: sku
    enableRbacAuthorization: false
    accessPolicies: [  
      for userObjectId in objectIds: {
        objectId: userObjectId
        permissions: {
          secrets: [
            'all'
          ]
        } 
      }
    ]
  }
}
