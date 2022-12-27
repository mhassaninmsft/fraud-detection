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


variable "pg_username" {
  type        = string
  default     = "citus"
  description = "username"
}

variable "pg_password" {
  type        = string
  description = "passowrd for pg"
}

variable "acr_name" {
  type        = string
  description = "passowrd for pg"
}

variable "container_name" {
  type        = string
  description = "passowrd for pg"
}

variable "eh_connection_string" {
  type        = string
  description = "passowrd for pg"
}
variable "acr_regitry_url" {
  type        = string
  description = "passowrd for pg"
}
variable "acr_image_name" {
  type        = string
  description = "passowrd for pg"
}
variable "acr_image_tag" {
  type        = string
  description = "passowrd for pg"
  default     = "latest"
}
