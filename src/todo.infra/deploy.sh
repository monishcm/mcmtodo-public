#!/bin/sh

PROJECT_PATH_TODO_UI='./src/todo.ui/todo.ui.csproj'
PROJECT_PATH_TODO_REPO='./src/todo.repo/todo.repo.csproj'
PROJECT_PATH_TODO_INFRA_BICEP='./src/todo.infra/azuredeploy.bicep'
PROJECT_PATH_TODO_INFRA_JSON='./src/todo.infra/azuredeploy.json'
PROJECT_OUT_PATH_TODO_UI='./out/todo.ui'
PROJECT_OUT_PATH_TODO_REPO='./out/todo.repo'
PROJECT_DIR_PATH_TODO_INFRA='./src/todo.infra/'
RESOURCE_GROUP='tododevops-dev'
PROJECT_PATH_TODO_INFRA_PARAM='./src/todo.infra/azuredeploy.parameters.dev.json'

# build and publish repository web api packages
dotnet restore $PROJECT_PATH_TODO_REPO
dotnet build $PROJECT_PATH_TODO_REPO --configuration Release 
dotnet publish $PROJECT_PATH_TODO_REPO -c Release -o $PROJECT_OUT_PATH_TODO_REPO

# build and publish UI web app packages
dotnet restore $PROJECT_PATH_TODO_UI
dotnet build $PROJECT_PATH_TODO_UI --configuration Release
dotnet publish $PROJECT_PATH_TODO_UI -c Release -o $PROJECT_OUT_PATH_TODO_UI

# build biceps
az bicep build --file $PROJECT_PATH_TODO_INFRA_BICEP

# deploy infra and extract web app names
outputs=$(az deployment group create -g $RESOURCE_GROUP --template-file $PROJECT_PATH_TODO_INFRA_JSON --parameters $PROJECT_PATH_TODO_INFRA_PARAM)
webapp_name_repo=$(echo $outputs | jq -r .properties.outputs.webapp_name_repo.value)
webapp_name_ui=$(echo $outputs | jq -r .properties.outputs.webapp_name_ui.value)

# zip packages for deployment
cd out/todo.repo; zip -r ../todo.repo.zip .; cd ../..
cd out/todo.ui; zip -r ../todo.ui.zip .; cd ../..

# Deploy repository web apis
az webapp deployment source config-zip --resource-group $RESOURCE_GROUP --name $webapp_name_repo --src out/todo.repo.zip

# Deploy UI web app
az webapp deployment source config-zip --resource-group $RESOURCE_GROUP --name $webapp_name_ui --src out/todo.ui.zip