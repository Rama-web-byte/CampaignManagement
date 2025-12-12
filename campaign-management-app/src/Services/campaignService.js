// src/api/campaignApi.js

const API_URL = "https://localhost:44340/api/Campaigns"; // Replace with your actual API endpoint

// src/Services/APIService.js
   const token=localStorage.getItem("token");
export const getCampaigns = async (page = 1, limit = 10, showActive = false) => {
    try {
        // Add the showActive parameter to the query string
     
        if(token==null)
        {
            throw new Error("User is not authenticated")
        }
        const response = await fetch(`${API_URL}?page=${page}&pageSize=${limit}&isActive=${showActive}`,
            {
                method:"GET",
                headers:{
                    "Content-Type":"application/json",
                    "Authorization": `Bearer ${token}`
                }
            }
        );
        if (!response.ok) throw new Error("Failed to fetch campaigns");
        const data = await response.json();
        return data;
    } catch (error) {
        console.log(error);
        console.error("Error fetching campaigns:", error);
        throw error;
    }
};




export const getProducts = async () => {
    try {
        const response = await fetch(`${API_URL}/activeProducts`,
            {
               method:"GET",
               headers:{
                "Content-Type":"application/json",
                "Authorization":`Bearer ${token}`
               }
            }
        ); // Adjust this endpoint according to your API
        if (!response.ok) throw new Error("Failed to fetch products");
        const data = await response.json();
        return data; // Return the list of products
    } catch (error) {
        console.error("Error fetching products:", error);
        throw error;
    }
};

export const createCampaign = async (campaignData) => {

console.log("campaignData:",campaignData)
  const response = await fetch(API_URL, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(campaignData),
  });

  if (response.status === 201) {
    // Created successfully
    return await response.json();
  }

  if (response.status === 400) {
    const errorData = await response.json();
    console.log(errorData?.message)
    throw new Error(errorData?.message || "Validation error");
  }

  if (response.status === 409) {
    const errorText = await response.text();
     console.log(errorText)
    throw new Error(errorText || "Conflict error");
  }

  throw new Error("Unexpected error occurred");
};


export const getCampaignById = async (id) => {
    try {
        const response = await fetch(`${API_URL}/${id}`,
        {
            method:"GET",
            headers:{
                "Content-Type":"application/json",
                "Authorization":`Bearer ${token}`

            }

        }
    );
        if (!response.ok) throw new Error("Failed to fetch campaign");
        return await response.json();
    } catch (error) {
        console.error("Error fetching campaign:", error);
        throw error;
    }
};


export const updateCampaign=async(id,campaign)=>
{
    try
    {
        const response=await fetch(`${API_URL}/${id}`,
            {
                method:"PUT",
                headers:
                {
                    "Content-Type":"application/json",
                    "Authorization":`Bearer ${token}`
                },
                body:JSON.stringify(
                    { 
                        campaignId:id,
                        campaignName:campaign.campaignName,
                        startDate:campaign.startDate,
                        endDate:campaign.endDate,
                        productId:campaign.productId

                    })
                
            });
        if(response.status===204) return true;
        if(!response.ok)throw new Error("Failed to update campaign");
        return await response.json();
    }
    catch(error)
    {
        console.error("Error updating campaign",error);
        throw error;
    }

};

export const deleteCampaign=async(id)=>{
     try
    {
       const response= await fetch(`${API_URL}/${id}`,
       {
            method:"DELETE",
            headers:
            {
                "Content-Type":"application/json",
                "Authorization":`Bearer ${token}`
            }
        }
      );

        if(response.status==204) return true;
        if(!response.ok) throw new Error("Failed to delete campaign");
        return await response.json();
    }
    catch(error)
    {
        console.error("Error deleting campaign",error);
        throw error;
    }

   };
