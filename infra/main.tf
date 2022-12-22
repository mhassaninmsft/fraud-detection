terraform {
  backend "azurerm" {
  }
  required_version = "~> 1.2"
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = ">= 3.0"
    }
    time = {
      source  = "hashicorp/time"
      version = ">= 0.9.1"
    }
  }
}
provider "azurerm" {
  features {
    api_management {
      purge_soft_delete_on_destroy = true
    }
  }
}

locals {
  prefix = "${var.resourceNamePrefix}${var.environment}${var.location}"
}
resource "azurerm_resource_group" "main_resource_group" {
  name     = "${var.resourceNamePrefix}-${var.environment}-${var.location}-rg"
  location = var.location
}
