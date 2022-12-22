### Manual Deployment

The following steps can be used to manually deploy the solution.

1. Reference `local.tfvars.template` to create and populate `local.tfvars`
within this directory.
2. References `backend.tfvars.template` to create and populate `backend.tfvars`
within this directory.
3. Perform the following set of commands:
    ``` bash
    # Login to Azure
    az login
    # alternatively use service principal login
    # az login --service-principal -u <app-id> -p <password-or-cert> --tenant <tenant>
    # Initialize the backend
    terraform init --backend-config=backend.tfvars
    # apply the changes
    terraform apply --var-file=local.tfvars
    ```

## Terraform Lock

It is recommended to submit the `.terraform.lock.hcl` to source control to
ensure that the providers version match on subsequent runs, moreover to make
sure that the same lock file work across platforms (Windows, Linux , Mac), we
should run the command

``` bash
terraform providers lock -platform=windows_amd64 -platform=darwin_amd64 -platform=linux_amd64
```
