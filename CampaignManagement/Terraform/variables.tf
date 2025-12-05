variable  "sql_admin"
{
    type=string
}

variable "sql_password"
{
    type=string
    sensitive=true
}

variable "jwt_screet"
{
    type=string
    sensitive=true
}