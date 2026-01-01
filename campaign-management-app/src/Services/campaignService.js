
//const API_URL = "https://localhost:44340/api/Campaigns";
const API_URL = `${process.env.REACT_APP_API_URL?.replace(/\/$/, "")}/api/Campaigns`;

// Always get fresh token
function getToken() {
  return localStorage.getItem("token");
}

export const getCampaigns = async (page = 1, limit = 10, showActive = false) => {
  try {
    const token = getToken();
    if (!token) throw new Error("User is not authenticated");

    const response = await fetch(
      `${API_URL}?page=${page}&pageSize=${limit}&isActive=${showActive}`,
      {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
      }
    );

    if (!response.ok) throw new Error("Failed to fetch campaigns");
    return await response.json();
  } catch (error) {
    console.error("Error fetching campaigns:", error);
    throw error;
  }
};

export const getProducts = async () => {
  try {
    const token = getToken();

    const response = await fetch(`${API_URL}/activeProducts`, {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
    });

    if (!response.ok) throw new Error("Failed to fetch products");
    return await response.json();
  } catch (error) {
    console.error("Error fetching products:", error);
    throw error;
  }
};

export const createCampaign = async (campaignData) => {
  const token = getToken();

  const response = await fetch(API_URL, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(campaignData),
  });

  if (response.status === 201) {
    return await response.json();
  }

  if (response.status === 400) {
    const errorData = await response.json();
    throw new Error(errorData?.message || "Validation error");
  }

  if (response.status === 409) {
    const errorText = await response.text();
    throw new Error(errorText || "Conflict error");
  }

  throw new Error("Unexpected error occurred");
};

export const getCampaignById = async (id) => {
  try {
    const token = getToken();

    const response = await fetch(`${API_URL}/${id}`, {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
    });

    if (!response.ok) throw new Error("Failed to fetch campaign");
    return await response.json();
  } catch (error) {
    console.error("Error fetching campaign:", error);
    throw error;
  }
};

export const updateCampaign = async (id, campaign) => {
  try {
    const token = getToken();

    const response = await fetch(`${API_URL}/${id}`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
      body: JSON.stringify({
        campaignId: id,
        campaignName: campaign.campaignName,
        startDate: campaign.startDate,
        endDate: campaign.endDate,
        productId: campaign.productId,
      }),
    });

    if (response.status === 204) return true;
    if (!response.ok) throw new Error("Failed to update campaign");

    return await response.json();
  } catch (error) {
    console.error("Error updating campaign", error);
    throw error;
  }
};

export const deleteCampaign = async (id) => {
  try {
    const token = getToken();

    const response = await fetch(`${API_URL}/${id}`, {
      method: "DELETE",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
    });

    if (response.status === 204) return true;
    if (!response.ok) throw new Error("Failed to delete campaign");

    return await response.json();
  } catch (error) {
    console.error("Error deleting campaign", error);
    throw error;
  }
};
