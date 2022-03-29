# GitHub Actions Demo

<details>
  <summary>Expand here to see build status</summary>

- [![main](https://github.com/microsoft/mcmtodo/actions/workflows/dotnet.0.yml/badge.svg)](https://github.com/microsoft/mcmtodo/actions/workflows/dotnet.0.yml)
- [![main](https://github.com/microsoft/mcmtodo/actions/workflows/dotnet.1.yml/badge.svg)](https://github.com/microsoft/mcmtodo/actions/workflows/dotnet.1.yml)
- [![main](https://github.com/microsoft/mcmtodo/actions/workflows/dotnet.2.yml/badge.svg)](https://github.com/microsoft/mcmtodo/actions/workflows/dotnet.2.yml)
- [![main](https://github.com/microsoft/mcmtodo/actions/workflows/dotnet.3.yml/badge.svg)](https://github.com/microsoft/mcmtodo/actions/workflows/dotnet.3.yml)
- [![main](https://github.com/microsoft/mcmtodo/actions/workflows/dotnet.4.yml/badge.svg)](https://github.com/microsoft/mcmtodo/actions/workflows/dotnet.4.yml)
- [![main](https://github.com/microsoft/mcmtodo/actions/workflows/dotnet.5.yml/badge.svg)](https://github.com/microsoft/mcmtodo/actions/workflows/dotnet.5.yml)
- [![main](https://github.com/microsoft/mcmtodo/actions/workflows/dotnet.6.yml/badge.svg)](https://github.com/microsoft/mcmtodo/actions/workflows/dotnet.6.yml)
- [![main](https://github.com/microsoft/mcmtodo/actions/workflows/dotnet.7.yml/badge.svg)](https://github.com/microsoft/mcmtodo/actions/workflows/dotnet.7.yml)
- [![main](https://github.com/microsoft/mcmtodo/actions/workflows/dotnet-codeql.yml/badge.svg)](https://github.com/microsoft/mcmtodo/actions/workflows/dotnet-codeql.yml)
- [![main](https://github.com/microsoft/mcmtodo/actions/workflows/dotnet.sonar.yml/badge.svg)](https://github.com/microsoft/mcmtodo/actions/workflows/dotnet.sonar.yml)
- [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=mcmtodo-project&metric=alert_status)](https://sonarcloud.io/dashboard?id=mcmtodo-project)

</details>

The demo showcases a stage by stage approach towards creating a sophisticated CD/CI pipeline in GitHub Actions. We start from a simple shell script for web deployment and convert it to a rich workflow by adding features in iterations.

The solution to be deployed is a Todo app which does no more than allowing one to add and read todo items. There is no authentication implemented to reduce complexity and focus on the CD/CI aspect.

## Application architecture

<img src="img/Architecture2.png?raw=true" width="400" />

## Infra architecture

<img src="img/infra.png?raw=true" width="600" />


## :soccer: Scenarios covered

- [x] Parallel Jobs
- [x] Publishing and sharing build artifacts between jobs
- [x] Fan-in / Fan-out
- [x] Passing values to dependent jobs and between non-dependent jobs
- [x] Creating and publishing releases on GitHub ([Releases](https://github.com/microsoft/mcmtodo/releases))
- [x] Auto versioning using [Git version](https://gitversion.net/)
- [x] Multi environment deployment with manual approval
- [x] Code analysis using CodeQL ([Security Overview](https://github.com/microsoft/mcmtodo/security))
- [x] Code analysis using Sonar Cloud
- [x] Azure deployment using biceps :muscle:

## :tornado: The workflows

<details>
<Summary>
To start with we have a simple shell script for Azure web deployment. The shell does 3 steps

- Builds and packages the Razor Web App and the Repository Web Api.
- Use az group deployment and biceps to create the azure infrastructure.
- Finally use webdeploy to deploys the apps.

</Summary>

<img src="img/deploy.sh.png?raw=true"/>

</details>

> [deploy.sh](src/todo.infra/deploy.sh)

### Iteration 0
<details>
<Summary>
Call the sh file from a basic GitHub action to deploy the application</Summary>

<img src="img/dotnet.0.png?raw=true" width="600" />

<img src="img/dotnet.0.yml.png?raw=true"/>

</details>

> [dotnet.0.yml](.github/workflows/dotnet.0.yml)

### Iteration 1

<details>
<Summary>
Extract individual steps from the sh file and use GitHub actions to execute them. The immediate advantage is the rich logging and manageability compared to using a script based deployment.</Summary>

<img src="img/dotnet.1.png?raw=true" width="600" />

<img src="img/dotnet.1.yml.png?raw=true"/>

</details>

> [dotnet.1.yml](.github/workflows/dotnet.1.yml)

### Iteration 2

<details>
<Summary>
Split the steps into independent jobs so tasks can run in parallel. For large projects this will drastically improve your build execution time. Also see how to pass build artifacts between jobs.

> Each job runs in their own independent VM. So we need to use a common share to pass artifacts.</Summary>

<img src="img/dotnet.2.png?raw=true" width="300" />

<img src="img/dotnet.2.yml.png?raw=true"/>

</details>

> [dotnet.2.yml](.github/workflows/dotnet.2.yml)

### Iteration 3

<details>
<Summary>
In the previous step, the web deployment would have failed if the build had not complete. A small step, add dependency between jobs to have more control on the execution.</Summary>

<img src="img/dotnet.3.png?raw=true" width="600" />

<img src="img/dotnet.3.yml.png?raw=true"/>

</details>

> [dotnet.3.yml](.github/workflows/dotnet.3.yml)

### Iteration 4

<details>
<Summary>
Now we do some more parallelism and use output variables to pass data between dependent jobs. Example: passing the Web URI after the infrastructure is deployed to the job that deploys the app itself.</Summary>

<img src="img/dotnet.4.png?raw=true" width="600" />

<img src="img/dotnet.4.yml.png?raw=true"/>

</details>

> [dotnet.4.yml](.github/workflows/dotnet.4.yml)

### Iteration 5

<details>
<Summary>
We can also use actions to Create and publish GitHub Releases. We use these tasks also to create a fan-in/fan-out design.</Summary>

<img src="img/dotnet.5.png?raw=true" width="800" />

<img src="img/dotnet.5.yml.png?raw=true"/>

</details>

> [dotnet.5.yml](.github/workflows/dotnet.5.yml)

### Iteration 6

<details>
<Summary>
In this stage we add an auto versioning component [Git version](https://gitversion.net/) which can predict the next version using the commit messages (example, having a breaking keyword in the commit message will bump the major version number).</Summary>

<img src="img/dotnet.6.png?raw=true" width="600" />

<img src="img/dotnet.6.yml.png?raw=true"/>

</details>

> [dotnet.6.yml](.github/workflows/dotnet.6.yml)

### Iteration 7

<details>
<Summary>
In the final stage we add GitHub Environments and approvals to deploy to multiple environments.</Summary>

<img src="img/dotnet.7.png?raw=true"/>

<img src="img/dotnet.7.yml.png?raw=true"/>

</details>

> [dotnet.7.yml](.github/workflows/dotnet.7.yml)

### Analyzers

See 2 examples of adding analyzers

<details>
<Summary>
1. GitHubs own CodeQL
    - Uses a matrix strategy to analyze both csharp and javascript. This is really easy to setup from the GitHub Security tab and I almost did no change to the auto generated yml file.</Summary>

<img src="img/dotnet.codeql.png?raw=true" width="300"/>

<img src="img/dotnet.codeql.yml.png?raw=true"/>

</details>

> [dotnet.codeql.yml](.github/workflows/dotnet-codeql.yml)

<details>
<Summary>
2. Sonar cloud
    - Uses sonar cloud. Again, the set up was very simple, in terms of taking the yml file from [here]('https://docs.sonarqube.org/latest/analysis/github-integration') and customizing it to the project.</Summary>

<img src="img/dotnet.sonar.png?raw=true" width="600"/>

<img src="img/dotnet.sonar.yml.png?raw=true"/>

</details>

> [dotnet.sonar.yml](.github/workflows/dotnet.sonar.yml)

## :memo: Issues / Notes

- [ ] Could not find a clean way to share data between jobs (that are not dependent on each other). Currently using cache but it requires a lot of plumbing work. I could not find a way to update/add data to an existing cache. As a result multiple cache had to be created. Cache also hold data between runs so you need a new key for every run which makes key management a bit tricky. Yet another way to share is by using the upload/download artifact feature but this will expose the data in the artifacts page of the build.
- [ ] Gated release only addresses manual approval. Also, could not find a way to block pull request based on build failure.
- [ ] Could not find a clean way to create sub-modules/templates. An option available is [composite](https://docs.github.com/en/actions/creating-actions/creating-a-composite-run-steps-action) runs but this is very limited. Another option is [workflow dispatch](https://github.blog/changelog/2020-07-06-github-actions-manual-triggers-with-workflow_dispatch/) which can call another workflow - again this is not targeted at modularizing the template so need to try this out to understand the pros and cons.
- [ ] [Self-hosted runners](https://docs.github.com/en/actions/hosting-your-own-runners/about-self-hosted-runners). I was not able to host multiple self-hosted runners in the same machine (my desktop). The run command throws an error if more than 1 runners from the same machine try to reach the repo. As a result I rely on the github-hosted runners at the cost of performance (most tools needs to be reinstalled for every run).

## :bulb: Things that could make actions more appealing

- A way to modularize templates. Also ability to associate modularized templates to environments will be a good addition.
- Creating and sharing variables between jobs and workflows.
- More integration in environments for checks and gates, ex. webhooks, app insights monitoring etc.. (similar to azure devops) but in a more _open sourcey_ way..
- Similar to azure policies that can be applied on ARM templates, it will be good to have a Policy engine that evaluates build templates. actions can be allowed and denied which makes it much more manageable for large organizations.

## :black_nib: Fork it

1. Fork the repo
2. Update the variables inside the yml files. Few yml files need ```RESOURCE_GROUP``` to be updated. Rest of them picks it up from the Secrets section 
3. Create a Azure Resource Group
4. Execute ```az ad sp create-for-rbac --name "<sp_name>" --role contributor --scopes /subscriptions/<subscription_id>/resourceGroups/<rg_name> --sdk-auth``` to create a service principal and copy the output JSON.
5. Paste it into GitHub Secrets under the name ```AZURE_CREDENTIALS```
6. Create a secrete ```PARAM_FILE``` with the relative path to param file (for this repo it will be ./src/todo.infra/azuredeploy.parameters.dev.json)
7. One more secret ```RESOURCE_GROUP``` with the resource name created in step 3

</br>

> GitHub Actions helps you with not just automating the CD/CI tasks but tasks in around the whole repo like, tracking issues, auto replying to comments and PR etc... This gives incredible opportunity for developers to automate repeated admin tasks.

</br>

> This page is not yet complete...
