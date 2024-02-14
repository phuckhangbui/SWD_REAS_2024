import React from "react";
import { GoogleLogin } from "@react-oauth/google";
import api from "../../api/axios";

const GoogleLogIn = () => {
  const handleOnSuccess = async (credentialResponse) => {
    try {
      console.log(credentialResponse);
      console.log(credentialResponse.credential);
      const idTokenString = String(credentialResponse.credential)

      const response = await api.post(
        "/api/Account/login/google",
        { idTokenString },
        {
          headers: {
            Authorization: `Bearer ${credentialResponse.credential}`,
            "Content-Type": "application/json",
          },
        }
      );
      return response;
    } catch (error) {
      console.log(error.response.data);
      console.log(error.response.status);
      console.log(error.response.headers);
    }
  };
  return (
    <div>
      <GoogleLogin
        onSuccess={handleOnSuccess}
        onError={() => {
          console.log("Login Failed");
        }}
      />
    </div>
  );
};

export default GoogleLogIn;
