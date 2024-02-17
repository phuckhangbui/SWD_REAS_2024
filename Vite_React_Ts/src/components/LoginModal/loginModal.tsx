import { useState } from "react";
import GoogleLogIn from "../GoogleLogIn/googleLogIn";

interface LoginModalProps {
  closeModal: () => void;
}

const LoginModal = ({ closeModal }: LoginModalProps) => {
  const [tabStatus, setTabStatus] = useState("user");

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
                  htmlFor="email"
                  className="block mb-2 text-sm font-medium text-gray-900 "
                >
                  Your email
                </label>
                <input
                  type="email"
                  name="email"
                  id="email"
                  className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white"
                  placeholder="name@company.com"
                  required
                />
              </div>
              <div>
                <label
                  htmlFor="password"
                  className="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                >
                  Your password
                </label>
                <input
                  type="password"
                  name="password"
                  id="password"
                  placeholder="••••••••"
                  className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-600 dark:border-gray-500 dark:placeholder-gray-400 dark:text-white"
                  required
                />
              </div>
              <div className="flex justify-between">
                <div className="flex items-start">
                  <div className="flex items-center h-5">
                    <input
                      id="remember"
                      type="checkbox"
                      value=""
                      className="w-4 h-4 border border-gray-300 rounded bg-gray-50 focus:ring-3 focus:ring-blue-300 dark:bg-gray-600 dark:border-gray-500 dark:focus:ring-blue-600 dark:ring-offset-gray-800 dark:focus:ring-offset-gray-800"
                      required
                    />
                  </div>
                  <label
                    htmlFor="remember"
                    className="ms-2 text-sm font-medium text-gray-900 dark:text-gray-300"
                  >
                    Remember me
                  </label>
                </div>
                <a
                  href="#"
                  className="text-sm text-blue-700 hover:underline dark:text-blue-500"
                >
                  Lost Password?
                </a>
              </div>
              <button
                type="submit"
                className="w-full text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
              >
                Login to your account
              </button>
              <div className="text-sm font-medium text-gray-500 dark:text-gray-300">
                Not registered?{" "}
                <a
                  href="#"
                  className="text-blue-700 hover:underline dark:text-blue-500"
                >
                  Create account
                </a>
              </div>
            </form>
          </div>
        </div>
        <div className={getActiveTabDetail("user")}>
          <div className="flex justify-center items-center p-36">
            <GoogleLogIn closeModal={closeModal}/>
          </div>
        </div>
      </div>
    </div>
  );
};

export default LoginModal;
