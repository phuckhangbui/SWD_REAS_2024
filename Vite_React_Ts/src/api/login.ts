import axios from "axios";
const baseUrl = process.env.REACT_APP_BACK_END_URL;

export const googleLogIn = async (idTokenString: string) => {
  try {
    console.log(idTokenString)
    const response = await axios.post(
      `${baseUrl}/api/Account/login/google`,
      { idTokenString },
      {
        headers: {
          Authorization: `Bearer ${idTokenString}`,
          "Content-Type": "application/json",
        },
      }
    );
    console.log(response);
    return response;
  } catch (error) {
    console.log("Error: " + error);
  }
};
