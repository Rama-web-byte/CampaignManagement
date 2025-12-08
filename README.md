# CampaignManagement API

![.NET](https://img.shields.io/badge/.NET-8.0-blue)
![Docker](https://img.shields.io/badge/Docker-Enabled-green)
![Azure](https://img.shields.io/badge/Azure-PAAS-orange)


A **RESTful API** for managing marketing campaigns and products, built with **.NET 8**, featuring CRUD operations, JWT authentication, rate-limiting, compression, telemetry, and deployment via Terraform and Docker.

---

## Table of Contents

1. [Project Overview](#project-overview)  
2. [Features](#features)  
3. [Architecture](#architecture)  
4. [Prerequisites](#prerequisites)  
5. [Setup & Run](#setup--run)  
6. [API Endpoints](#api-endpoints)  
7. [Authentication](#authentication)  
8. [Rate Limiting](#rate-limiting)  
9. [Logging & Telemetry](#logging--telemetry)  
10. [Docker](#docker)  
11. [Terraform Infrastructure](#terraform-infrastructure)  


---

## Project Overview

CampaignManagement API allows management of **campaigns** 
- Campaigns have **start and end dates**, with `IsActive` computed dynamically.  
- Users are authenticated via **JWT tokens** and roles determine access levels.

---

## Features

- Full CRUD for **Campaigns** 
- JWT-based Authentication and Role-based Authorization  
- **Rate-limiting** per user and per endpoint  
- **Gzip compression** for API responses  
- **Application Insights** for telemetry and logging  
- **Dockerized** for containerized deployments  
- **Terraform scripts** for Azure resource provisioning  
- FluentValidation for **input validation**  

---

- **Backend:** .NET 8 Web API  
- **Database:**SQL
- **Monitoring:** Azure Application Insights  
- **Rate Limiting:** User-based & Write operations  

---

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)  
- [Docker](https://www.docker.com/)  
- [Terraform](https://developer.hashicorp.com/terraform/downloads)  
- Azure subscription (Free Tier for demo purposes)  

---

## Setup & Run

### Clone the Repository

```bash
git clone <repository-url>
cd CampaignManagement

## Architecture
Configure Environment Variables

JWT Secret

Database connection string

Application Insights connection string

Run Locally
dotnet restore
dotnet build
dotnet run

Docker
docker build -t campaign-api .
docker run -p 8080:8080 campaign-api

API Endpoints
Method	Endpoint	Description	Auth Required
GET	/api/campaigns	List campaigns (paginated)	Yes
GET	/api/campaigns/{id}	Get a campaign by ID	Yes
POST	/api/campaigns	Create a new campaign	Admin/CampaignOwner
PUT	/api/campaigns/{id}	Update campaign	Admin/CampaignOwner
DELETE	/api/campaigns/{id}	Delete campaign	Admin/CampaignOwner
GET	/api/users	List all users	Admin
POST	/api/users/login	Authenticate user	No
Authentication

JWT token-based authentication

Roles: Admin, CampaignOwner, User

Include Authorization: Bearer <token> header in requests

Rate Limiting

User-based: Limits requests per user per minute

Write operations: Separate limit to prevent abuse

Returns 429 Too Many Requests when limits are exceeded

Logging & Telemetry

Application Insights integration

Automatic tracking of exceptions, requests, and custom events

UserId and CampaignId included in logs for tracing

Example: Track custom event in code

_telemetryClient.TrackEvent("CampaignCreated", new Dictionary<string, string>
{
    { "CampaignName", newModel.CampaignName },
    { "ProductId", newModel.ProductId.ToString() }
});

Docker

Multi-stage Dockerfile for efficient builds

Exposes ports 8080 and 8081

Environment-based configuration via environment variables

Terraform Infrastructure

Resource Group: rg-campaign-demo

Application Insights: Telemetry & logging

