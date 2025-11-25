import React, { useState, useEffect } from 'react';
import DatePicker from 'react-datepicker';
import 'react-datepicker/dist/react-datepicker.css'; // Import DatePicker styles
import { getProducts, createCampaign } from "../Services/APIService"; // Import the API methods

const CreateCampaign = () => {
  const [campaignsName, setCampaignsName] = useState(''); // Changed to campaignsName
  const [productId, setProductId] = useState(''); // Renamed from product
  const [products, setProducts] = useState([]);
  const [startDate, setStartDate] = useState(null);
  const [endDate, setEndDate] = useState(null);
  const [loading, setLoading] = useState(true); // Loading state for products
  const [error, setError] = useState(null); // State for error messages
  const [success, setSuccess] = useState(null); // State for success messages

  // Fetch products from API when component mounts
  useEffect(() => {
    const fetchProducts = async () => {
      try {
        setLoading(true);
        const fetchedProducts = await getProducts();
        setProducts(fetchedProducts);
      } catch (error) {
        console.error("Error fetching products:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchProducts();
  }, []);

  const handleCreateCampaign = async () => {
    // Construct the campaignData object to match the expected format
    const campaignData = {
      campaignsName,  // Use the correct key
      productId,      // Use productId instead of product
      startDate: startDate ? startDate.toISOString() : null, // Convert to ISO format
      endDate: endDate ? endDate.toISOString() : null,       // Convert to ISO format
    };

    try {
      const result = await createCampaign(campaignData); // Call the API service to create the campaign
      console.log("Campaign Created:", result);
      setSuccess("Campaign created successfully!"); // Set success message
      setError(null); // Clear any previous error message
      // Reset fields after successful creation
      setCampaignsName('');  // Reset campaignsName
      setProductId('');      // Reset productId
      setStartDate(null);
      setEndDate(null);
    } catch (error) {
      setError("Failed to create campaign."); // Set error message
      console.error("Error creating campaign:", error);
    }
  };

  return (
    <div style={{ maxWidth: 600, margin: 'auto', padding: '20px', border: '1px solid #ccc', borderRadius: '8px' }}>
      <h2>Create New Campaign</h2>

      {/* Success and Error Messages */}
      {success && <p style={{ color: 'green' }}>{success}</p>}
      {error && <p style={{ color: 'red' }}>{error}</p>}

      {/* Campaign Name Input */}
      <div style={{ marginBottom: '16px' }}>
        <label htmlFor="campaignsName">Campaign Name</label>
        <input
          type="text"
          id="campaignsName"
          value={campaignsName}  // Use campaignsName
          onChange={(e) => setCampaignsName(e.target.value)} // Update handler for campaignsName
          style={{ width: '100%', padding: '8px', marginTop: '8px' }}
          required
        />
      </div>

      {/* Product Dropdown Input */}
      <div style={{ marginBottom: '16px' }}>
        <label htmlFor="product">Product</label>
        <select
          id="product"
          value={productId} // Use productId
          onChange={(e) => setProductId(e.target.value)} // Update handler for productId
          style={{ width: '100%', padding: '8px', marginTop: '8px' }}
          required
          disabled={loading || products.length === 0}
        >
          <option value="">Select a product</option>
          {loading ? 
          (
            <option disabled>Loading products...</option>
          ) : (
            products.map((prod) => (
              <option key={prod.productId} value={prod.productId}>
                {prod.productName}
              </option>
            ))
          )}
        </select>
      </div>

      {/* Start Date Input */}
      <div style={{ marginBottom: '16px' }}>
        <label htmlFor="startDate">Start Date</label>
        <DatePicker
          id="startDate"
          selected={startDate}
          onChange={(date) => setStartDate(date)}
          dateFormat="dd/MM/yyyy"
          placeholderText="Select start date"
          style={{ width: '100%', padding: '8px', marginTop: '8px' }}
          required
        />
      </div>

      {/* End Date Input */}
      <div style={{ marginBottom: '16px' }}>
        <label htmlFor="endDate">End Date</label>
        <DatePicker
          id="endDate"
          selected={endDate}
          onChange={(date) => setEndDate(date)}
          dateFormat="dd/MM/yyyy"
          placeholderText="Select end date"
          style={{ width: '100%', padding: '8px', marginTop: '8px' }}
          required
        />
      </div>

      {/* Create Campaign Button */}
      <button
        onClick={handleCreateCampaign}
        style={{ width: '100%', padding: '12px', backgroundColor: '#007bff', color: '#fff', border: 'none', borderRadius: '4px', cursor: 'pointer' }}
      >
        Create Campaign
      </button>

      {/* Discard Changes Button */}
      <button
        onClick={() => {
          setCampaignsName('');  // Reset campaignsName
          setProductId('');      // Reset productId
          setStartDate(null);
          setEndDate(null);
        }}
        style={{ width: '100%', padding: '12px', backgroundColor: '#f44336', color: '#fff', border: 'none', borderRadius: '4px', cursor: 'pointer', marginTop: '8px' }}
      >
        Discard Changes
      </button>
    </div>
  );
};

export default CreateCampaign;
