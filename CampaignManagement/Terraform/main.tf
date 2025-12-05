provider "azurerm"
{
    features{}
}

# Fetch current Azure client info
data "azurerm_client_config" "current" {}

# Resource group

resource "azurerm_resource_group" "rg"
{
    name="rg-campaign-demo"
    location="southindia"
}

# App Service plan(Free tier)
resource "azurerm_app_service_plan" "app_plan"
{
    name ="campaign-api-plan"
    location=azurerm_resource_group.rg.location
    resource_group_name=azurerm_resource_group.rg.name
    sku
    {
        tier="Free"
        size="F1"
    }
}

#App Service
resource "azurerm_app_service" "app"
{
    name="campaign-api-demo"
    location=azurerm_resource_group.rg.location
    resource_group_name=azurerm_resource_group.rg.name
    app_service_plan_id=azurerm_app_service_plan.app_plan.id
    site_config
    {
        dotnet_framework_version="v8.0"
    }
}

# SQL Server

resource "azurerm_sql_server" "sql"
{
    name= "campaignsqlserver"
    resource_group_name=azurerm_resource_group.rg.name
    location=azurerm_resource_group.rg.location
    version="12.0"
    administrator_login=var.sql_admin
    administrator_login_password=var.sql_password  

}

#SQL Database

resource "azurerm_sql_database" "db"
{
    name= "CampaignDB"
    resource_group_name=azurerm_resource_group.rg.name
    server_name=azurerm_sql_server.sql.name
    sku_name="Basic"
    max_size_gb=0.25
}

# Application Insights

resource "azurerm_application_insights" "ai"
{
    name="campaign-api-ai"
    location=azurerm_resource_group.rg.location
    resource_group_name=azurerm_resource_group.rg.name
    application_type="web"
}


# Key Vault
resource "azurerm_key_vault" "kv"
{
    name = "campaign-api-kv"
    location=azurerm_resource_group.rg.location
    resource_group_name=azurerm_resource_group.rg.name
    sku_name="standard"
    tenant_id=data.azurerm_client_config.current.tenant_id
    soft_delete_enabled=true
}