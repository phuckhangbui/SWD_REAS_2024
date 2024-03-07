import axios from "axios";
const baseUrl = process.env.REACT_APP_BACK_END_URL;

export const getAuctionAllAdmin = async (token : string) => {
  try {
    const fetchData = await axios.get<AuctionAdmin[]>(`${baseUrl}/api/Auction/auctions/all`, 
    {
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json",
        },
      });
    const response = fetchData.data;
    return response;
  } catch (error) {
    console.log("Error: " + error);
  }
};

export const getAuctionCompleteAdmin = async (token : string) => {
    try {
      const fetchData = await axios.get<AuctionAdmin[]>(`${baseUrl}/api/Auction/auctions/complete`, 
      {
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          },
        });
      const response = fetchData.data;
      return response;
    } catch (error) {
      console.log("Error: " + error);
    }
  };


export const getAuctionAllAdminById = async (id: Number | undefined, token : string) => {
  try {
    const fetchData = await axios.get<AuctionDetailAllAdmin>(
      `${baseUrl}/api/Auction/auctions/all/detail/${id}`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json",
        },
      }
    );
    const response = fetchData.data;
    return response;
  } catch (error) {
    console.log("Error: " + error);
  }
};

export const getAuctionCompleteAdminById = async (id: Number | undefined, token : string) => {
    try {
      const fetchData = await axios.get<AuctionDetailCompleteAdmin>(
        `${baseUrl}/api/Auction/auctions/all/complete/${id}`,
        {
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          },
        }
      );
      const response = fetchData.data;
      return response;
    } catch (error) {
      console.log("Error: " + error);
    }
  };

  export const getRealForDeposit = async (token : string) => {
    try {
      const fetchData = await axios.get<RealForDeposit[]>(`${baseUrl}/api/Auction/realfordeposit`, 
      {
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          },
        });
      const response = fetchData.data;
      return response;
    } catch (error) {
      console.log("Error: " + error);
    }
  };

  export const getUserForDeposit = async (token : string, id : number) => {
    try {
      const fetchData = await axios.get<DepositAmountUser[]>(`${baseUrl}/api/Auction/realfordeposit/${id}`, 
      {
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          },
        });
      const response = fetchData.data;
      return response;
    } catch (error) {
      console.log("Error: " + error);
    }
  };


export const addAuction = async ({
    AccountCreateId,
    ReasId,
    DateStart,
    DateEnd,
    FloorBid
}:AuctionCreate, token: string) => {
    try {
        const param ={
          AccountCreateId,
          ReasId,
          DateStart,
          DateEnd,
          FloorBid
        }
      const fetchData = await axios.post<Message>(
        `${baseUrl}/api/Auction/deposit/create`,
        param,
        {
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          },
        }
      );
      const response = fetchData.data;
      return response;
    } catch (error) {
      console.log("Error: " + error);
    }
  };

//   export const updateNews = async ({
//     newsContent,
//     newsSumary,
//     newsTitle,
//     thumbnail,
//     dateCreated,
//     newsId
// }:newsUpdate, token: string) => {
//     try {
//         const param ={
//             newsContent, newsSumary, newsTitle, thumbnail, dateCreated, newsId
//         }
//       const fetchData = await axios.post<Message>(
//         `${baseUrl}/api/admin/news/update`,
//         param,
//         {
//           headers: {
//             Authorization: `Bearer ${token}`,
//             "Content-Type": "application/json",
//           },
//         }
//       );
//       const response = fetchData.data;
//       return response;
//     } catch (error) {
//       console.log("Error: " + error);
//     }
//   };