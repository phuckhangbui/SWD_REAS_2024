import { GoogleLogin } from "@react-oauth/google";
import { useNavigate } from "react-router-dom";

interface GoogleLogInProps {
    closeModal: () => void;
  }

const GoogleLogIn = ({ closeModal }: GoogleLogInProps) => {
  const navigate = useNavigate();
  const handleOnSuccess = (credentialResponse: object) => {
    try {
        console.log(credentialResponse)
        closeModal()
    } catch (error) {}
  };

  return (
    <div>
      <div>
        <GoogleLogin
          onSuccess={handleOnSuccess}
          onError={() => {
            console.log("Login Failed");
          }}
        />
      </div>
    </div>
  );
};

export default GoogleLogIn;
