import { useState } from "react";
import GoogleLogIn from "../GoogleLogIn/googleLogIn";

interface LoginModalProps {
  closeModal: () => void;
}

const LoginModal = ({ closeModal }: LoginModalProps) => {
  const [tabStatus, setTabStatus] = useState("user");
  const [showPassword, setShowPassword] = useState(false);
  const [error, setError] = useState<string | null>(null);

  // Change the tab index
  const toggleTab = (type: string) => {
    setTabStatus(type);
  };

  // Tab button status
  const getActiveTab = (type: string) => {
    return `${
      type === tabStatus
        ? "text-mainBlue border-mainBlue font-bold h-0"
        : "border-transparent hover:border-gray-900 max-w-full group-hover:max-w-full transition-all duration-500 bg-sky-600 h-0"
    } text-xl border-b-2 rounded-t-lg `;
  };

  // Changing the tab description
  const getActiveTabDetail = (type: string) => {
    return `${type === tabStatus ? "" : "hidden"} mt-2 space-y-4 `;
  };

  const handleSeePassword = (e: React.FormEvent<HTMLButtonElement>) => {
    e.preventDefault();
    setShowPassword((prevStatus) => !prevStatus);
  };

  return (
    <div className="relative p-4 w-full max-w-md max-h-full">
      <div className="relative bg-white rounded-lg shadow ">
        <div className="p-4 md:p-5 border-b rounded-t text-center ">
          <h3 className="text-2xl  font-bold text-gray-900 ">Sign in</h3>
        </div>
        <div className=" grid grid-cols-2 transition duration-300 group mb-8">
          <button
            className={getActiveTab("user")}
            onClick={() => toggleTab("user")}
          >
            <div className="mt-2">User</div>
          </button>
          <button
            className={getActiveTab("staff")}
            onClick={() => toggleTab("staff")}
          >
            <div className="mt-2">Staff</div>
          </button>
        </div>
        <div className={getActiveTabDetail("staff")}>
          <div className="p-4 md:p-5">
            <form className="space-y-4" action="#">
              <div>
                <label
                  htmlFor="Username"
                  className="block mb-2 text-sm font-medium text-gray-900 "
                >
                  Username:
                </label>
                <div>
                  <input
                    type="text"
                    name="Username"
                    id="Username"
                    className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-mainBlue focus:border-mainBlue block w-full p-2.5 "
                    placeholder="Type your username"
                  />
                </div>
              </div>

              <label
                htmlFor="password"
                className="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
              >
                Password:
              </label>
              <div className="flex justify-end items-center mt-0">
                <input
                  type={showPassword ? "text" : "password"}
                  name="password"
                  id="password"
                  placeholder="Type your password"
                  className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-mainBlue focus:border-mainBlue block w-full p-2.5"
                />
                <button className="absolute mr-3" onClick={handleSeePassword}>
                  <svg
                    className="w-6 h-6 text-gray-900 dark:text-white "
                    aria-hidden="true"
                    xmlns="http://www.w3.org/2000/svg"
                    fill="none"
                    viewBox="0 0 24 24"
                  >
                    <path
                      stroke="currentColor"
                      stroke-width="2"
                      d="M21 12c0 1.2-4 6-9 6s-9-4.8-9-6c0-1.2 4-6 9-6s9 4.8 9 6Z"
                    />
                    <path
                      stroke="currentColor"
                      stroke-width="2"
                      d="M15 12a3 3 0 1 1-6 0 3 3 0 0 1 6 0Z"
                    />
                  </svg>
                </button>
              </div>
              <div className="flex justify-end">
                <a href="#" className="text-sm text-mainBlue hover:underline">
                  Lost Password?
                </a>
              </div>
              <button
                type="submit"
                className="w-full text-white bg-mainBlue hover:bg-darkerMainBlue focus:ring-4 focus:outline-none focus:ring-mainBlue font-medium rounded-lg text-sm px-5 py-2.5 text-center "
              >
                Login
              </button>
            </form>
          </div>
        </div>
        <div className={getActiveTabDetail("user")}>
          <div className="flex justify-center items-center p-8">
            <GoogleLogIn closeModal={closeModal} />
          </div>
        </div>
      </div>
    </div>
  );
};

export default LoginModal;
