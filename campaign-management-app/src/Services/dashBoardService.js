//const API_URL = "https://localhost:44340/api/Dashboard";
const API_URL = "/api/Dashboard";
console.log(API_URL);


function getToken()
{
    return localStorage.getItem("token");
}


export const getCampaignStatusSummary=async()=>
{

    try{
        const token=getToken();
        if(!token) throw new Error("User is not authenticated");

        const response=await fetch(`${API_URL}/GetCampaignStatus`,
        {
            method:"GET",
            headers:{
                "Content-Type":"application/json",
                 Authorization:`Bearer ${token}`,
            },
        }
    
    );

      
       if (!response.ok) throw new Error("Failed to fetch campaignstatus");
            return await response.json();
        } 
        
        catch (error) 
        {
            console.error("Error fetching campaignstatuslist:", error);
            throw error;
        }
    
 };   
          
    
export const getCampaignsByProduct=async()=>
{

    try{
        const token=getToken();
        if(!token) throw new Error("User is not authenticated");

        const response=await fetch(`${API_URL}/GetCampaignProducts`,
        {
            method:"GET",
            headers:{
                "Content-Type":"application/json",
                 Authorization:`Bearer ${token}`,
            },
        });
       if (!response.ok) throw new Error("Failed to fetch campaignproducts");
            return await response.json();
        } 
        
        catch (error) 
        {
            console.error("Error fetching campaignproducts:", error);
            throw error;
        }
    
 };  