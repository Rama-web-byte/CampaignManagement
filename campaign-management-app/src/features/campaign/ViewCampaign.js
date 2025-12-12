import React,{useState,useEffect}from "react";
import { useParams,useNavigate } from "react-router-dom";
import { getCampaignById } from "../../Services/campaignService";
import Layout from "../../Components/Layout";

const ViewCampaign=()=>
{
    const{id}=useParams();
    const[campaign,setCampaign]=useState(null);
    const[loading,setLoading]=useState(true);
    const navigate=useNavigate();

    useEffect(()=>
    {
        const fetchCampaign=async()=>
        {
            try{
            const data=await getCampaignById(id);
            setCampaign(data);
            }
            catch(error)
            {
                console.error("Error fetching campaign:",error)
            }
            finally{
                setLoading(false);
            }
        };
        fetchCampaign();
    },[id]);

    if (loading) return<Layout>Loading...</Layout>
    if(!campaign) return<Layout>No campaign found</Layout>

    return(
<Layout>
    <div className="p-6 bg-white rounded shadow">
        <h2 className="text-2xl font-bold mb-4">{campaign.campaignName}</h2>
        <p><strong>Product:</strong>{campaign.productName}</p>
        <p><strong>Start Date:</strong>{" "}
        {new Date(campaign.startDate).toLocaleDateString()}
        </p>
        <p><strong>End Date:</strong>{" "}
        {new Date(campaign.endDate).toLocaleDateString()}
        </p>
        <p><strong>Active:</strong>
        {campaign.isActive?"Yes":"No"}
        </p>
        <button onClick={()=>navigate("/campaigns")} className="px-4 py-2 rounded bg-gray-300 text-gray-800 hover:bg-gray-400 hover:text-white transition font-medium">
            Back To Campaigns</button>
    </div>
</Layout>
    );
};
export default ViewCampaign;


