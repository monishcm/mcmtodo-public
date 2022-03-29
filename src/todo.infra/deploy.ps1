az deployment group create --name addappserviceplan --resource-group tododevops-dev --template-file azuredeploy.bicep
az deployment group create -g tododevops-dev --template-file ./azuredeploy.bicep --parameters ./azuredeploy.parameters.dev.json

az ad sp create-for-rbac --name "<sp_name>" --role contributor --scopes /subscriptions/<subscription_id>/resourceGroups/tododevops-dev --sdk-auth