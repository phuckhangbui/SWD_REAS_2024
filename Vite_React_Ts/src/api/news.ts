import axios from "axios";
const baseUrl = process.env.REACT_APP_BACK_END_URL;

export const getNewsHome = async () => {
  try {
    const fetchData = await axios.get<news[]>(`${baseUrl}/api/home/news`);
    const response = fetchData.data;
    return response;
  } catch (error) {
    console.log("Error: " + error);
  }
};
export const searchNews = async ({
  pageNumber,
  pageSize,
  keyWord,
}: searchNews) => {
  try {
    const param = {
      pageNumber,
      pageSize,
      keyWord,
    };
    const fetchData = await axios.post<news[]>(
      `${baseUrl}/api/home/news/search`,
      { params: param }
    );
    const response = fetchData.data;
    return response;
  } catch (error) {
    console.log("Error: " + error);
  }
};
