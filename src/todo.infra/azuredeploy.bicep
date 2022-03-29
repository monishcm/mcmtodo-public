@description('The Azure Cosmos DB database account name.')
param databaseAccountName string

param dbName string = 'todolist'
param containerName string = 'todoitems'
param databaseAccountTier string = 'Standard'
param partitionKey string = '/userId'

@description('The name of the App Service Plan that will host the Web App.')
param appSvcPlanName string

@description('The instance size of the App Service Plan.')
param svcPlanSize string = 'B1'

@description('The pricing tier of the App Service plan.')
param svcPlanSku string = 'Basic'

@description('The name of the Repo Web API.')
param webAppName_repo string

@description('The name of the UI Web App.')
param webAppName_ui string

@description('Location for all resources.')
param location string = resourceGroup().location

param appInsights_name string

resource cosmosDBAccount 'Microsoft.DocumentDB/databaseAccounts@2021-04-15' = {
  name: databaseAccountName
  location: location
  properties: {
    databaseAccountOfferType: databaseAccountTier
    locations: [
      {
        locationName: location
        failoverPriority: 0
        isZoneRedundant: true
      }
    ]
    capabilities: [
      {
        name: 'EnableServerless'
      }
    ]
  }
}

resource appinsights_todo 'microsoft.insights/components@2020-02-02-preview' = {
  name: appInsights_name
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
  }
}

resource appSvcPlan 'Microsoft.Web/serverfarms@2018-02-01' = {
  name: appSvcPlanName
  location: location
  sku: {
    name: svcPlanSize
    tier: svcPlanSku
    capacity: 1
  }
}

resource webApp_repo 'Microsoft.Web/Sites@2018-11-01' = {
  name: webAppName_repo
  location: location
  properties: {
    httpsOnly: true
    serverFarmId: appSvcPlan.id
    siteConfig: {
      phpVersion: 'off'
      appSettings: [
        {
          name: 'CosmosDb:ConnectionString'
          value: listConnectionStrings(resourceId('Microsoft.DocumentDB/databaseAccounts', cosmosDBAccount.name), '2020-04-01').connectionStrings[0].connectionString
        }
        {
          name: 'CosmosDb:DatabaseName'
          value: dbName
        }
        {
          name: 'CosmosDb:CollectionName'
          value: containerName
        }
        {
          name: 'CosmosDb:PartitionKey'
          value: partitionKey
        }
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: reference('Microsoft.Insights/components/${appinsights_todo.name}').InstrumentationKey
        }
      ]
    }
  }
  dependsOn: [
    appSvcPlan
    cosmosDBAccount
  ]
}

resource webApp_ui 'Microsoft.Web/Sites@2018-11-01' = {
  name: webAppName_ui
  location: location
  properties: {
    httpsOnly: true
    serverFarmId: appSvcPlan.id
    siteConfig: {
      phpVersion: 'off'
      appSettings: [
        {
          name: 'TodoRepoUrl'
          value: 'https://${webApp_repo.name}.azurewebsites.net'
        }
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: reference('Microsoft.Insights/components/${appinsights_todo.name}').InstrumentationKey
        }
      ]
    }
  }
  dependsOn: [
    appSvcPlan
  ]
}

output webapp_name_repo string = webAppName_repo
output webapp_name_ui string = webAppName_ui
