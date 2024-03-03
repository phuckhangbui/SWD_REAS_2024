import { ReactNode, createContext, useEffect, useState } from "react";

interface UserProviderProps {
  children: ReactNode;
}

interface UserContextType {
  user: loginUser | undefined;
  token: string | undefined;
  login: (user: loginUser, token: string) => void;
  logout: () => void;
  isAuth: () => boolean;
}

export const UserContext = createContext<UserContextType>({
  user: undefined,
  token: undefined,
  login: () => {},
  logout: () => {},
  isAuth: () => false,
});

const UserProvider = ({ children }: UserProviderProps) => {
  const [user, setUser] = useState<loginUser | undefined>(undefined);
  const [token, setToken] = useState<string | undefined>(undefined);

  const login = (user: loginUser, token: string) => {
    const stringUser = JSON.stringify(user);
    localStorage.setItem("user", stringUser);
    localStorage.setItem("token", token);
    setUser(user);
    setToken(token);
  };

  useEffect(() => {
    const storageToken = localStorage.getItem("token");
    const storageUser = localStorage.getItem("user");
    const parseStorageUser = JSON.parse(storageUser as string);

    if (storageToken && storageUser) {
      setUser(parseStorageUser);
      setToken(storageToken);
    }
  }, []);

  const logout = () => {
    setUser(undefined);
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
    <UserContext.Provider value={{ user, token, login, logout, isAuth }}>
      {children}
    </UserContext.Provider>
  );
};

export default UserProvider;