variable "resourceNamePrefix" {
  type        = string
  description = "Globally unique key to guarntee environment uniqueness"
}

variable "location" {
  type        = string
  default     = "eastus"
  description = "The Azure location name for deployed resources"
}

variable "environment" {
  type        = string
  default     = "local"
  description = "Description for env"
}
