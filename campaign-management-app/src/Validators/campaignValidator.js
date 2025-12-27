export const validateCampaignField = ({
  field,
  value,
  form,
  mode,
  today,
  maxEndDate,
  status
}) => {
  switch (field) {
    case "campaignName":
      if (!value || value.trim().length === 0)
        return "Campaign name is required";

      if (value.length < 3)
        return "Campaign name must be at least 3 characters";

      if (value.length > 50)
        return "Campaign name cannot exceed 50 characters";

      return "";

    case "startDate":
      if (!value) return "Start date is required";

      if (mode === "update" && (status === "ongoing" || status === "expired"))
        return "Start Date cannot be changed";

      if (new Date(value) < today)
        return "Start date cannot be in the past";

      return "";

    case "endDate":
      if (!value) return "End date is required";

      if (new Date(value) < new Date(form.startDate))
        return "End date must be after Start date";

      if (new Date(value) > maxEndDate)
        return "End date cannot exceed 12 months";

      return "";

    default:
      return "";
  }
};
