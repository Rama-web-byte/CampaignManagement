output "app_service_default_hostname"
{
value=azurerm_app_service.app.default_site_hostname
}

output "sql_connection_string"
{
    value=azurerm_sql_database.db.id
}

output "keyvault_uri"
{
    value=azurerm_key_vault.kv.vault_uri
}

output "application_insights_instrumentation_key"
{
    value=azurerm_application_insights.ai.instrumentation_key

}