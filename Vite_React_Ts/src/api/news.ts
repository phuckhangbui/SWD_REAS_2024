import axios from "axios";
import news from "../interface/news";
const baseUrl = process.env.REACT_APP_BACK_END_URL;

export const getNewsHome = async () => {
  try {
    const fetchData = await axios.get<news[]>(
      `${baseUrl}/api/home/news`
    );
    const response = fetchData.data;
    return response;
  } catch (error) {
    console.log("Error: " + error);
  }
};
export const searchNews = async (
  pageNumber: number,
  pageSize: number,
  keyWord: string,
) => {
  try {
    const param = {
      pageNumber,
      pageSize,
      keyWord,
    };
    const fetchData = await axios.get<news[]>(
      `${baseUrl}/api/home/news/search`,
      { params: param }
    );
    const response = fetchData.data;
    return response;
  } catch (error) {
    console.log("Error: " + error);
  }
};
