import React,{useState,useEffect} from 'react';
import {getCampaigns} from "../../Services/campaignService";
import {Table,TableContainer,TableRow,TableCell,TableHead,TableBody}from '@mui/material';
const WorkOut=()=>
{
const[campaigns,setCampaigns]=useState([]);
const[loading,setLoading]=useState(true);

    const fetchcampaigns=async()=>
    {
            try
            {
                setLoading(true);
                const response=await getCampaigns();
               
                setCampaigns(response[0]?.campaigns || []);
                
            }
            catch(error)
            {
                 throw new error;
             }
             finally
             {
                setLoading(false);
             }

    };

    useEffect(()=>{fetchcampaigns();},[]);
  

    return(
       <TableContainer>
        <Table>
         <TableHead>
            <TableRow>
                <TableCell>CampaignName</TableCell>
                <TableCell>ProductName</TableCell>
                <TableCell>StartDate</TableCell>
                <TableCell>EndDate</TableCell>
            </TableRow>
         </TableHead>
         <TableBody>  
            { 
            campaigns.length >0 ? (
              campaigns.map
              (
                (campaign, index)=> 
                  (
                           <TableRow key={campaign.campaignId || campaign.productName||index}>
                            <TableCell>{campaign.campaignsName}</TableCell>
                            <TableCell>{campaign.productName}</TableCell>
                            <TableCell>{new Date(campaign.startDate).toLocaleDateString()}</TableCell>
                            <TableCell>{new Date(campaign.endDate).toLocaleDateString()}</TableCell>
                            </TableRow>
                            )
                      )):
                        ( <TableRow>
                            <TableCell colSpan={4} align="center">No Data Found</TableCell>
                            </TableRow>)}
         </TableBody>
        </Table>
       </TableContainer>


    );


};
export default WorkOut;