import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import Layout from "../../Components/Layout";
import { getCampaignById, updateCampaign } from "../../Services/campaignService";
import { validateCampaignField } from "../../Validators/campaignValidator";

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

  // 🔴 Field-level errors
  const [errors, setErrors] = useState({});
  // 🔴 Form-level message
  const [message, setMessage] = useState(null);

  const today = new Date();
  today.setHours(0, 0, 0, 0);
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

  // 🔴 Field validation
  const validateField = (field, value) => {
    const message = validateCampaignField({
      field,
      value,
      form,
      mode: "update", // use update mode for edit campaign
      today,
      maxEndDate,
      status
    });

    setErrors(prev => ({ ...prev, [field]: message }));
    return !message;
  };

  // 🔴 Validate all fields before submit
  const validateForm = () => {
  let isValid = true;

  const changedFields = {
    campaignName: form.campaignName !== original.campaignName,
    startDate: form.startDate !== original.startDate,
    endDate: form.endDate !== original.endDate
  };

  Object.entries(changedFields).forEach(([field, changed]) => {
    if (!changed) return;

    const valid = validateField(field, form[field]);
    if (!valid) isValid = false;
  });

  return isValid;
};


  const handleChange = (e) => {
    const { name, value } = e.target;
    validateField(name, value);
    setForm(prev => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    setMessage(null);

    if (!validateForm()) return;

    try {
      await updateCampaign(id, form);
      setMessage("Campaign updated successfully");
      navigate("/campaigns");
    } catch (error) {
      setErrors(prev => ({ ...prev, form: error.message }));
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

        {/* Form-level message */}
        {message && <p className="text-green-600 mb-3">{message}</p>}
        {errors.form && <p className="text-red-600 mb-3">{errors.form}</p>}

        <form onSubmit={handleSubmit} className="space-y-4">

          {/* Campaign Name */}
          <div>
            <label className="block font-semibold">Campaign Name</label>
            <input
              type="text"
              name="campaignName"
              value={form.campaignName}
              onChange={handleChange}
              className={`border p-2 w-full rounded ${errors.campaignName ? "border-red-500" : ""}`}
              required
            />
            {errors.campaignName && <p className="text-red-600 text-sm mt-1">{errors.campaignName}</p>}
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
              className={`border p-2 w-full rounded ${status !== "future" ? "bg-gray-100 cursor-not-allowed" : ""} ${errors.startDate ? "border-red-500" : ""}`}
              required
            />
            {errors.startDate && <p className="text-red-600 text-sm mt-1">{errors.startDate}</p>}
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
              className={`border p-2 w-full rounded ${status === "expired" ? "bg-gray-100 cursor-not-allowed" : ""} ${errors.endDate ? "border-red-500" : ""}`}
              required
            />
            {errors.endDate && <p className="text-red-600 text-sm mt-1">{errors.endDate}</p>}
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

          {/* Buttons */}
          <div className="flex space-x-4">
            <button
              type="submit"
              disabled={!isFormChanged}
              className={`px-4 py-2 rounded ${isFormChanged ? "bg-emerald-600 text-white hover:bg-emerald-700 transition" : "bg-gray-300 text-gray-700 cursor-not-allowed"}`}
            >
              Update
            </button>

            <button
              type="button"
              onClick={() => navigate("/campaigns")}
              className="px-4 py-2 rounded bg-gray-300 text-gray-800 hover:bg-gray-400 hover:text-white transition font-medium"
            >
              Back To Campaigns
            </button>
          </div>
        </form>
      </div>
    </Layout>
  );
};

export default EditCampaign;
