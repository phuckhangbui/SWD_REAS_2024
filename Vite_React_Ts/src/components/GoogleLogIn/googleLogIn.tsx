import { GoogleLogin } from "@react-oauth/google";
import { useNavigate } from "react-router-dom";
import { googleLogIn } from "../../api/login";

interface GoogleLogInProps {
  closeModal: () => void;
  // onSuccess: () => void
}

const GoogleLogIn = ({ closeModal }: GoogleLogInProps) => {
  const navigate = useNavigate();
  const handleOnSuccess = async (credentialResponse: any) => {
    try {
      console.log(credentialResponse);
      const idTokenString = String(credentialResponse.credential);
      const user = await googleLogIn(idTokenString);
      
      closeModal();
    } catch (error) {
      console.log(error)
    }
  };
  const handleFailed = async () => {

  }

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
