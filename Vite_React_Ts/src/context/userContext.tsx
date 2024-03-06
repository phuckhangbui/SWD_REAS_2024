import { ReactNode, createContext, useEffect, useState } from "react";

interface UserProviderProps {
  children: ReactNode;
}

interface UserContextType {
  userRole: number | undefined;
  token: string | undefined;
  login: (user: loginUser, token: string) => void;
  logout: () => void;
  isAuth: () => boolean;
}

export const UserContext = createContext<UserContextType>({
  userRole: undefined,
  token: undefined,
  login: () => {},
  logout: () => {},
  isAuth: () => false,
});

const UserProvider = ({ children }: UserProviderProps) => {
  const [userRole, setUserRole] = useState<number | undefined>(undefined);
  const [token, setToken] = useState<string | undefined>(undefined);

  useEffect(() => {
    try {
      const getLocalData = async () => {
        const storageToken = localStorage.getItem("token");
        const storageUser = localStorage.getItem("user");
        if (storageToken && storageUser) {
          const parseStorageUser = JSON.parse(storageUser as string);
          setUserRole(parseStorageUser.roleId);
          setToken(storageToken);
        }
      };
      getLocalData();
      // console.log("User Role: ", userRole);
    } catch (error) {
      console.log(error);
    }
  }, []);

  // useEffect(() => {
  //   console.log("User Role: ", userRole);
  // }, [userRole]);

  const login = (user: loginUser, token: string) => {
    const stringUser = JSON.stringify(user);
    localStorage.setItem("user", stringUser);
    localStorage.setItem("token", token);
    setUserRole(user?.roleId);
    setToken(token);
  };

  const logout = () => {
    setUserRole(undefined);
    setToken(undefined);
    localStorage.removeItem("user");
    localStorage.removeItem("token");
  };

  const isAuth = () => {
    const storageToken = localStorage.getItem("token");
    if (storageToken && storageToken !== undefined) {
      return true;
    } else {
      return false;
    }
  };

  return (
    <UserContext.Provider value={{ userRole, token, login, logout, isAuth }}>
      {children}
    </UserContext.Provider>
  );
};

export default UserProvider;
