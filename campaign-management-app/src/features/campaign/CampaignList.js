import React, { useEffect, useState } from "react";
import { getCampaigns } from "../../Services/campaignService";
import Layout from "../../Components/Layout";
import { useNavigate,Link } from "react-router-dom";
import { deleteCampaign } from "../../Services/campaignService";

const CampaignList = () => {
  const [campaigns, setCampaigns] = useState([]);
  const [totalPages, setTotalPages] = useState(1);
  const [currentPage, setCurrentPage] = useState(1);

  const [pageSize, setPageSize] = useState(10);
  const navigate=useNavigate();

  const fetchCampaigns = async (page) => {
    try {
      const response = await getCampaigns(page, pageSize);
      setCampaigns(response.campaigns || []);
      setCurrentPage(response.currentPage || 1);
      setTotalPages(response.totalPages || 1);
    } catch (error) {
      console.error("Error fetching campaigns:", error);
    }
  };

  useEffect(() => {
    fetchCampaigns(currentPage);
  }, [currentPage]);

  const handlePageChange = (page) => setCurrentPage(page);

  const handleView=(id)=>
  {
   navigate(`/campaign/view/${id}`)
  }

  const handleEdit=(id)=>{
    navigate(`/campaign/edit/${id}`)
  }

  const handleDelete=async(id)=>
  {
    const confirmed=window.confirm("Are you sure you want to delete this campaign");
    if(!confirmed) return;

    try
    {
    await deleteCampaign(id);
    setCampaigns((prev)=>prev.filter((c)=>c.campaignId!==id));
    alert("campaign deleted successfully")
    }
    catch(error)
    {
    alert("Error deleting campaign: " +error.message)
    }
  }

  

  return (
   <Layout>
  <div className="min-h-screen bg-gray-50 p-6">
    {/* Create Campaign Button */}
   <div className="flex justify-end mb-4">
  <Link
    to="/create-campaign"
    className="text-indigo-500 font-semibold hover:underline transition-colors duration-200"
  >
    Create New Campaign
  </Link>
</div>



    {/* Table */}
    <div className="overflow-x-auto bg-white rounded-xl shadow border border-gray-200">
      <table className="w-full text-sm">
        <thead className="bg-gray-100 sticky top-0 z-10">
          <tr>
            <th className="px-6 py-3 text-left text-gray-700 font-semibold uppercase tracking-wider border-b border-gray-300">Name</th>
            <th className="px-6 py-3 text-left text-gray-700 font-semibold uppercase tracking-wider border-b border-gray-300">Product</th>
            <th className="px-6 py-3 text-left text-gray-700 font-semibold uppercase tracking-wider border-b border-gray-300">Start Date</th>
            <th className="px-6 py-3 text-left text-gray-700 font-semibold uppercase tracking-wider border-b border-gray-300">End Date</th>
            <th className="px-6 py-3 text-left text-gray-700 font-semibold uppercase tracking-wider border-b border-gray-300">Actions</th>
          </tr>
        </thead>
        <tbody className="divide-y divide-gray-200">
          {campaigns.length > 0 ? (
            campaigns.map((c) => (
              <tr key={c.campaignId} className="hover:bg-gray-50 transition-colors">
                <td className="px-6 py-3 text-gray-800">{c.campaignName}</td>
                <td className="px-6 py-3 text-gray-800">{c.productName}</td>
                <td className="px-6 py-3 text-gray-600">{new Date(c.startDate).toLocaleDateString()}</td>
                <td className="px-6 py-3 text-gray-600">{new Date(c.endDate).toLocaleDateString()}</td>
                <td className="px-6 py-3">
                  <div className="flex items-center gap-x-2">
                    <button onClick={() => handleView(c.campaignId)} className="px-2 py-1 rounded bg-indigo-600 text-white hover:bg-indigo-700 transition text-xs">View</button>
                    <button onClick={() => handleEdit(c.campaignId)} className="px-2 py-1 rounded bg-emerald-600 text-white hover:bg-emerald-700 transition text-xs">Edit</button>
                    <button onClick={() => handleDelete(c.campaignId)} className="px-2 py-1 rounded bg-red-600 text-white hover:bg-red-700 transition text-xs">Delete</button>
                  </div>
                </td>
              </tr>
            ))
          ) : (
            <tr>
              <td colSpan={5} className="px-6 py-4 text-center text-gray-500">No campaigns found.</td>
            </tr>
          )}
        </tbody>
      </table>

      {/* Pagination */}
      <div className="flex justify-between items-center px-6 py-4 bg-gray-50 border-t border-gray-200">
        <span className="text-sm text-gray-600">Page {currentPage} of {totalPages}</span>
        <div className="flex space-x-2">
          {Array.from({ length: totalPages }, (_, i) => (
            <button
              key={i + 1}
              onClick={() => handlePageChange(i + 1)}
              className={`px-3 py-1 rounded-lg text-sm font-medium ${
                currentPage === i + 1
                  ? "bg-indigo-600 text-white shadow"
                  : "bg-gray-100 text-gray-700 hover:bg-gray-200"
              }`}
            >
              {i + 1}
            </button>
          ))}
        </div>
      </div>
    </div>
  </div>
</Layout>

  );
};

export default CampaignList;
