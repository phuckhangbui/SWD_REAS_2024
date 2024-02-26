import axios from "axios";
import realEstate from "../interface/realEstate";
const baseUrl = process.env.REACT_APP_BACK_END_URL;

interface searchProps {
  pageNumber: number;
  pageSize: number;
  reasName: string;
  reasPriceFrom: string;
  reasPriceTo: string;
  reasStatus: number;
}

export const getRealEstateHome = async () => {
  try {
    const fetchData = await axios.get<realEstate[]>(
      `${baseUrl}/api/home/real_estate`
    );
    const response = fetchData.data;
    return response;
  } catch (error) {
    console.log("Error: " + error);
  }
};
export const searchRealEstate = async ({
  pageNumber,
  pageSize,
  reasName,
  reasPriceFrom,
  reasPriceTo,
  reasStatus,
}: searchProps) => {
  try {
    const param = {
      pageNumber,
      pageSize,
      reasName,
      reasPriceFrom,
      reasPriceTo,
      reasStatus,
    };
    const fetchData = await axios.post<realEstate[]>(
      `${baseUrl}/api/home/real_estate/search`,
      param
    );
    const response = fetchData.data;
    return response;
  } catch (error) {
    console.log("Error: " + error);
  }
};
