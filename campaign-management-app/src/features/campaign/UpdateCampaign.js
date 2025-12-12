import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import Layout from "../../Components/Layout";
import { getCampaignById, updateCampaign } from "../../Services/campaignService";

const EditCampaign = () => {
  const { id } = useParams();
  const navigate = useNavigate();

  const [form, setForm] = useState({
    campaignId: "",
    campaignName: "",
    startDate: "",
    endDate: "",
    productId: "",
    productName: ""
  });
  const [original, setOriginal] = useState(null);
  const [status, setStatus] = useState("future");

  const today = new Date();
  const todayStr = today.toISOString().split("T")[0];

  const maxEndDate = new Date();
  maxEndDate.setMonth(today.getMonth() + 12);
  const maxEndDateStr = maxEndDate.toISOString().split("T")[0];

  // Load campaign once
  useEffect(() => {
    const loadCampaign = async () => {
      try {
        const data = await getCampaignById(id);
        const startDateStr = data.startDate.split("T")[0];
        const endDateStr = data.endDate.split("T")[0];

        setForm({
          campaignId: data.campaignId,
          campaignName: data.campaignName,
          startDate: startDateStr,
          endDate: endDateStr,
          productId: data.productId,
          productName: data.productName
        });

        setOriginal({
          campaignName: data.campaignName,
          startDate: startDateStr,
          endDate: endDateStr
        });

        const start = new Date(startDateStr);
        const end = new Date(endDateStr);
        if (end < today) setStatus("expired");
        else if (start <= today && end >= today) setStatus("ongoing");
        else setStatus("future");
      } catch (error) {
        console.error("Error loading campaign", error);
      }
    };
    loadCampaign();
  }, [id]);

  if (!original) {
    return (
      <Layout>
        <div className="p-6">Loading...</div>
      </Layout>
    );
  }

  // Validation helper
  const validateField = (name, value) => {
    if (name === "startDate") {
      if (status === "ongoing" || status === "expired") {
        alert("Cannot change Start Date for ongoing or expired campaigns");
        return false;
      }
      if (status === "future" && new Date(value) < today) {
        alert("Start Date cannot be in the past");
        return false;
      }
    }

    if (name === "endDate") {
      const start = new Date(form.startDate);
      const newEnd = new Date(value);

      if (status === "expired") {
        alert("Cannot change End Date for expired campaigns");
        return false;
      }
      if (newEnd < start) {
        alert("End Date cannot be before Start Date");
        return false;
      }
      if (newEnd > maxEndDate) {
        alert("End Date cannot be more than 12 months in the future");
        return false;
      }
      if (status === "ongoing" && newEnd < today) {
        alert("End Date cannot be in the past for ongoing campaign");
        return false;
      }
    }
    return true;
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    if (!validateField(name, value)) return;
    setForm((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    const start = new Date(form.startDate);
    const end = new Date(form.endDate);

    const isStartChanged = form.startDate !== original.startDate;
    const isEndChanged = form.endDate !== original.endDate;

    // Final validations
    if (status === "future" && isStartChanged && start < today) {
      alert("Start Date cannot be in the past");
      return;
    }
    if (isEndChanged && end < start) {
      alert("End Date cannot be before Start Date");
      return;
    }
    if (isEndChanged && end > maxEndDate) {
      alert("End Date cannot be more than 12 months in the future");
      return;
    }
    if ((status === "ongoing" || status === "expired") && isEndChanged && end < today) {
      alert("End Date cannot be in the past for ongoing or expired campaign");
      return;
    }

    try {
      await updateCampaign(id, form);
      alert("Campaign updated successfully");
      navigate("/campaigns");
    } catch (error) {
      alert("Update failed: " + error.message);
    }
  };

  const isFormChanged =
    form.campaignName !== original.campaignName ||
    form.startDate !== original.startDate ||
    form.endDate !== original.endDate;

  return (
    <Layout>
      <div className="p-6 max-w-xl mx-auto bg-white rounded shadow">
        <h2 className="text-2xl mb-4 font-bold">Edit Campaign</h2>
        <form onSubmit={handleSubmit} className="space-y-4">
          {/* Campaign Name */}
          <div>
            <label className="block font-semibold">Campaign Name</label>
            <input
              type="text"
              name="campaignName"
              value={form.campaignName}
              onChange={handleChange}
              className="border p-2 w-full rounded"
              required
            />
          </div>

          {/* Start Date */}
          <div>
            <label className="block font-semibold">Start Date</label>
            <input
              type="date"
              name="startDate"
              value={form.startDate}
              min={todayStr}
              onChange={handleChange}
              disabled={status !== "future"}
              className={`border p-2 w-full rounded ${
                status !== "future" ? "bg-gray-100 cursor-not-allowed" : ""
              }`}
              required
            />
          </div>

          {/* End Date */}
          <div>
            <label className="block font-semibold">End Date</label>
            <input
              type="date"
              name="endDate"
              value={form.endDate}
              min={status === "ongoing" ? todayStr : form.startDate}
              max={maxEndDateStr}
              onChange={handleChange}
              disabled={status === "expired"}
              className={`border p-2 w-full rounded ${
                status === "expired" ? "bg-gray-100 cursor-not-allowed" : ""
              }`}
              required
            />
          </div>

          {/* Product */}
          <div>
            <label className="block font-semibold">Product</label>
            <input
              type="text"
              value={form.productName}
              disabled
              className="border p-2 w-full rounded bg-gray-100 cursor-not-allowed"
            />
          </div>
          <div className="flex space-x-4">
          <button
            type="submit"
            disabled={!isFormChanged}
            className={`px-4 py-2 rounded ${
              isFormChanged
                ? "bg-emerald-600 text-white hover:bg-emerald-700 transition"
                : "bg-gray-300 text-gray-700 cursor-not-allowed"
            }`}
          >
            Update
          </button>

           <button onClick={()=>navigate("/campaigns")} className="px-4 py-2 rounded bg-gray-300 text-gray-800 hover:bg-gray-400 hover:text-white transition font-medium">
            Back To Campaigns</button>
            </div>
        </form>
      </div>
    </Layout>
  );
};

export default EditCampaign;