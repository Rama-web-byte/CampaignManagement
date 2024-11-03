// src/api/campaignApi.js

const API_URL = "https://localhost:64291/api/Campaigns"; // Replace with your actual API endpoint

// src/Services/APIService.js

export const getCampaigns = async (page = 1, limit = 10, showActive = false) => {
    try {
        // Add the showActive parameter to the query string
        const response = await fetch(`${API_URL}?page=${page}&pageSize=${limit}&isActive=${showActive}`);
        if (!response.ok) throw new Error("Failed to fetch campaigns");
        const data = await response.json();
        return data;
    } catch (error) {
        console.log(error);
        console.error("Error fetching campaigns:", error);
        throw error;
    }
};


export const getCampaignById = async (id) => {
    try {
        const response = await fetch(`${API_URL}/${id}`);
        if (!response.ok) throw new Error("Failed to fetch campaign");
        return await response.json();
    } catch (error) {
        console.error("Error fetching campaign:", error);
        throw error;
    }
};

export const getProducts = async () => {
    try {
        const response = await fetch(`${API_URL}/activeProducts`); // Adjust this endpoint according to your API
        if (!response.ok) throw new Error("Failed to fetch products");
        const data = await response.json();
        return data; // Return the list of products
    } catch (error) {
        console.error("Error fetching products:", error);
        throw error;
    }
};

export const createCampaign = async (campaignData) => {
    try {
        console.log("campaignData",campaignData)
        const response = await fetch(API_URL, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(campaignData),
        });
        if (!response.ok) throw new Error("Failed to create campaign");
        return await response.json();
    } catch (error) {
        console.error("Error creating campaign:", error);
        throw error;
    }
};
