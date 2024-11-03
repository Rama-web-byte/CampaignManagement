import React, { useEffect, useState } from "react";
import { getCampaigns } from "../Services/APIService"; // Make sure this function supports pagination
import {
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    Pagination,
} from "@mui/material";
import '../Styles/CampaignList.css';

const CampaignList = () => {
    const [campaigns, setCampaigns] = useState([]); // To store campaigns
    const [totalCount, setTotalCount] = useState(0); // Total number of campaigns from API
    const [currentPage, setCurrentPage] = useState(1); // Current page number
    const [pageSize, setPageSize] = useState(5); // Page size defined in service

    // Function to fetch campaigns based on current page and page size
    const fetchCampaigns = async (page) => {
        try {
            const response = await getCampaigns(page, pageSize); // Fetch from API
            const data = response[0]; // Assuming response is structured as an array
            setCampaigns(data.campaigns); // Set fetched campaigns
            setTotalCount(data.totalCount); // Total count from API
        } catch (error) {
            console.error("Error fetching campaigns:", error);
        }
    };

    // Effect to fetch campaigns on mount and page change
    useEffect(() => {
        fetchCampaigns(currentPage); // Fetch campaigns based on current page
    }, [currentPage]); // Dependency on currentPage

    // Calculate the total pages based on total count and page size
    const totalPages = Math.ceil(totalCount / pageSize);

    // Handle page change
    const handlePageChange = (event, value) => {
        setCurrentPage(value); // Set new page number
    };

    return (
        <TableContainer>
            {/* Campaigns Table */}
            <Table>
                <TableHead>
                    <TableRow>
                        <TableCell className="table-heading">Name</TableCell>
                        <TableCell className="table-heading">Product</TableCell>
                        <TableCell className="table-heading">Start Date</TableCell>
                        <TableCell className="table-heading">End Date</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {campaigns.length > 0 ? (
                        campaigns.map((campaign) => (
                            <TableRow key={campaign.campaignId}>
                                <TableCell>{campaign.campaignsName}</TableCell>
                                <TableCell>{campaign.productName}</TableCell>
                                <TableCell>{new Date(campaign.startDate).toLocaleDateString()}</TableCell>
                                <TableCell>{new Date(campaign.endDate).toLocaleDateString()}</TableCell>
                            </TableRow>
                        ))
                    ) : (
                        <TableRow>
                            <TableCell colSpan={4} align="center">
                                No campaigns found.
                            </TableCell>
                        </TableRow>
                    )}
                </TableBody>
            </Table>

            {/* Pagination Component */}
            <Pagination
                count={totalPages} // Total number of pages based on total campaigns
                page={currentPage} // Current page
                onChange={handlePageChange} // Handle page change
                color="primary"
                variant="outlined"
                shape="rounded"
                siblingCount={1} // Optional: adjust for appearance
            />
        </TableContainer>
    );
};

export default CampaignList;
