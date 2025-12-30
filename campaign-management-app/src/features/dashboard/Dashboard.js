import React, { useEffect, useState, useMemo } from "react";
import {
  BarChart,
  Bar,
  XAxis,
  YAxis,
  Tooltip,
  ResponsiveContainer,
  CartesianGrid,
  PieChart,
  Pie,
  Cell,
  Legend
} from "recharts";
import Layout from "../../Components/Layout";
import {
  getCampaignStatusSummary,
  getCampaignsByProduct
} from "../../Services/dashBoardService";
// Optional: import toast if using react-toastify
// import { toast } from "react-toastify";

const COLORS = [
  "#0088FE", "#00C49F", "#FFBB28", "#FF8042", "#A569BD",
  "#FF6384", "#FF9F40", "#36A2EB", "#FFCD56", "#4BC0C0"
];

const Dashboard = () => {
  const [statusSummary, setStatusSummary] = useState([]);
  const [campaignsByProduct, setCampaignsByProduct] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadDashboard();
  }, []);

  const loadDashboard = async () => {
    try {
      const statusDataArray = await getCampaignStatusSummary();
      const statusData = statusDataArray[0];
      const productData = await getCampaignsByProduct();

      setStatusSummary([
        { name: "Active", value: statusData.activeCampaigns },
        { name: "Upcoming", value: statusData.upcomingCampaigns },
        { name: "Expired", value: statusData.expiredCampaigns }
      ]);

      setCampaignsByProduct(productData);
    } catch (error) {
      console.error("Error loading dashboard:", error);
      // toast.error("Failed to load dashboard data");
    } finally {
      setLoading(false);
    }
  };

  const totalCampaigns = useMemo(() => 
    statusSummary.reduce((sum, item) => sum + item.value, 0), 
    [statusSummary]
  );

  if (loading) return <p className="p-6 text-gray-500">Loading dashboard...</p>;

  return (
    <Layout>
      <div className="p-6 space-y-8">

        {/* KPI CARDS */}
        <div className="grid grid-cols-4 gap-4 md:grid-cols-2 sm:grid-cols-1">
          <Kpi title="Active" value={statusSummary[0]?.value} color="blue" />
          <Kpi title="Upcoming" value={statusSummary[1]?.value} color="yellow" />
          <Kpi title="Expired" value={statusSummary[2]?.value} color="red" />
          <Kpi title="Total" value={totalCampaigns} color="green" />
        </div>

        {/* STATUS CHART */}
        <ChartCard title="Campaign Status Overview">
          {statusSummary.length === 0 ? (
            <p className="text-sm text-gray-500">No campaign status data available.</p>
          ) : (
            <ResponsiveContainer width="100%" height={300}>
              <BarChart
                data={statusSummary}
                margin={{ top: 20, right: 30, left: 0, bottom: 0 }}
              >
                <CartesianGrid strokeDasharray="3 3" />
                <XAxis dataKey="name" />
                <YAxis />
                <Tooltip formatter={(value, name) => [`${value} campaigns`, name]} />
                <Bar dataKey="value" fill="#4f46e5" barSize={50} />
              </BarChart>
            </ResponsiveContainer>
          )}
        </ChartCard>

        {/* PRODUCT PIE CHART */}
        <ChartCard title="Campaigns by Product">
          {campaignsByProduct.length === 0 ? (
            <p className="text-sm text-gray-500">No product campaign data available.</p>
          ) : (
            <ResponsiveContainer width="100%" height={350}>
              <PieChart>
                <Pie
                  data={campaignsByProduct}
                  dataKey="campaignCount"
                  nameKey="productName"
                  cx="50%"
                  cy="50%"
                  outerRadius={100}
                  label={({ name, value }) =>
                    `${name.length > 20 ? name.slice(0, 20) + "…" : name}: ${value}`
                  }
                >
                  {campaignsByProduct.map((entry, index) => (
                    <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
                  ))}
                </Pie>
                <Tooltip formatter={(value, name) => [`${value} campaigns`, name]} />
                <Legend
                  layout="vertical"
                  verticalAlign="middle"
                  align="right"
                  wrapperStyle={{ maxHeight: 300, overflowY: "auto", fontSize: 12 }}
                />
              </PieChart>
            </ResponsiveContainer>
          )}
        </ChartCard>

      </div>
    </Layout>
  );
};

/* ---------- Reusable Components ---------- */

const ChartCard = ({ title, children }) => (
  <div className="bg-white p-4 rounded shadow">
    <h3 className="font-semibold mb-4">{title}</h3>
    {children}
  </div>
);

const Kpi = ({ title, value, color }) => {
  const bgColor = {
    blue: "bg-blue-100 text-blue-700",
    yellow: "bg-yellow-100 text-yellow-700",
    red: "bg-red-100 text-red-700",
    green: "bg-green-100 text-green-700"
  }[color] || "bg-gray-100 text-gray-700";

  return (
    <div className={`rounded shadow p-4 text-center ${bgColor}`}>
      <p className="text-sm">{title}</p>
      <p className="text-2xl font-bold">{value ?? 0}</p>
    </div>
  );
};

export default Dashboard;