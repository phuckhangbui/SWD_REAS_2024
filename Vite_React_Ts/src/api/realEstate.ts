import axios from "axios";
const baseUrl = process.env.REACT_APP_BACK_END_URL;

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
}: searchRealEstate) => {
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

export const getRealEstateById = async (id: number) => {
  try {
    const fetchData = await axios.get<realEstateDetail>(
      `${baseUrl}/api/home/real_estate/detail/${id}`
    );
    const response = fetchData.data;
    return response;
  } catch (error) {
    console.log("Error: " + error);
  }
};
