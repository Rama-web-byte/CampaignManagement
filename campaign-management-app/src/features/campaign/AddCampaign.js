import React, { useState, useEffect } from "react";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import { getProducts, createCampaign } from "../../Services/campaignService";
import { useNavigate } from "react-router-dom";
import Layout from "../../Components/Layout";

const CreateCampaign = () => {
  const navigate = useNavigate();

  const [campaignName, setCampaignName] = useState("");
  const [productId, setProductId] = useState("");
  const [products, setProducts] = useState([]);
  const [startDate, setStartDate] = useState(null);
  const [endDate, setEndDate] = useState(null);

  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [success, setSuccess] = useState(null);

  // Fetch products when component mounts
  useEffect(() => {
    const fetchProducts = async () => {
      try {
        setLoading(true);
        const fetchedProducts = await getProducts();
        setProducts(fetchedProducts || []);
      } catch {
        setError("Failed to load products.");
      } finally {
        setLoading(false);
      }
    };

    fetchProducts();
  }, []);

  const handleCreateCampaign = async () => {
    if (!campaignName || !productId || !startDate || !endDate) {
      setError("Please fill all fields.");
      setSuccess(null);
      return;
    }

    const campaignData = {
      campaignName,
      productId,
      startDate: startDate.toISOString(),
      endDate: endDate.toISOString(),
    };

    try {
      const result = await createCampaign(campaignData);
      setSuccess(`Campaign "${result.campaignName}" created successfully!`);
      setError(null);

      // Optionally, reset form
      setCampaignName("");
      setProductId("");
      setStartDate(null);
      setEndDate(null);

      // Navigate back to campaign list after 1.5s
      setTimeout(() => navigate("/campaigns"), 1500);
    } catch (err) {
      setError(err.message);
      setSuccess(null);
    }
  };

  return (
    <Layout>
      <div className="max-w-xl mx-auto bg-white shadow-md p-6 rounded-lg">
        <div className="flex justify-between items-center mb-6">
          <h2 className="text-2xl font-bold text-gray-800">Create New Campaign</h2>
          <button
            onClick={() => navigate("/campaigns")}
            className="px-4 py-2 bg-gray-200 text-gray-800 rounded hover:bg-gray-300 transition"
          >
            Back
          </button>
        </div>

        {success && <p className="text-green-600 mb-3">{success}</p>}
        {error && <p className="text-red-600 mb-3">{error}</p>}

        {/* Campaign Name */}
        <div className="mb-4">
          <label className="block text-gray-700">Campaign Name</label>
          <input
            type="text"
            value={campaignName}
            onChange={(e) => setCampaignName(e.target.value)}
            className="w-full p-2 border rounded mt-1"
          />
        </div>

        {/* Product */}
        <div className="mb-4">
          <label className="block text-gray-700">Product</label>
          <select
            value={productId}
            onChange={(e) => setProductId(e.target.value)}
            className="w-full p-2 border rounded mt-1"
            disabled={loading}
          >
            <option value="">Select a product</option>
            {loading ? (
              <option disabled>Loading...</option>
            ) : products.length === 0 ? (
              <option disabled>No products found</option>
            ) : (
              products.map((p) => (
                <option key={p.productId} value={p.productId}>
                  {p.productName}
                </option>
              ))
            )}
          </select>
        </div>

        {/* Start Date */}
        <div className="mb-4">
          <label className="block text-gray-700">Start Date</label>
          <DatePicker
            selected={startDate}
            onChange={(date) => setStartDate(date)}
            className="w-full p-2 border rounded mt-1"
            dateFormat="dd/MM/yyyy"
            placeholderText="Select start date"
          />
        </div>

        {/* End Date */}
        <div className="mb-4">
          <label className="block text-gray-700">End Date</label>
          <DatePicker
            selected={endDate}
            onChange={(date) => setEndDate(date)}
            className="w-full p-2 border rounded mt-1"
            dateFormat="dd/MM/yyyy"
            placeholderText="Select end date"
          />
        </div>

        {/* Create Button */}
     
  {/* Create Campaign */}
 <div className="flex justify-start gap-3 mt-6">
  {/* Create Campaign */}
  <button
    onClick={handleCreateCampaign}
    className="px-4 py-2 bg-amber-500 text-white font-semibold rounded hover:bg-amber-600 transition"
  >
    Create
  </button>

  {/* Discard Changes */}
  <button
    onClick={() => {
      setCampaignName("");
      setProductId("");
      setStartDate(null);
      setEndDate(null);
    }}
    className="px-4 py-2 bg-red-500 text-white rounded hover:bg-red-600 transition"
  >
    Discard
  </button>
</div>

</div>

  
    </Layout>
  );
};

export default CreateCampaign;
