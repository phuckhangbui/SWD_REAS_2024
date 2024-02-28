import "./App.css";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import PageNotFound from "./pages/PageNotFound";
import { AdminLayout } from "./pages/Admin/AdminLayout";
import AdminDashboard from "./pages/Admin/AdminDashboard";
import AuctionOngoing from "./pages/Admin/AdminAuctionOngoing";
import StaffList from "./pages/Admin/StaffList";
import MemberList from "./pages/Admin/MemberList";
import AuctionComplete from "./pages/Admin/AdminAuctionComplete";
import AuctionDetail from "./pages/Admin/AdminAuctionDetail";
import HomePage from "./pages/Member/HomePage/homePage";
import RealEstatePage from "./pages/Member/RealEstatePage/realEstatePage";
import HelpPage from "./pages/Member/HelpPage/helpPage";
import MemberLayout from "./pages/Member/memberLayout";
import AuctionPage from "./pages/Member/AuctionPage/auctionPage";
import NewsPage from "./pages/Member/NewsPage/newsPage";
import SellPage from "./pages/Member/SellPage/sellPage";

function App() {

  

  return (
    <div className="App">
      <Router>
        <Routes>
          <Route path="/" element={<MemberLayout />}>
            <Route index element={<HomePage />} />
            <Route path="/realEstate" element={<RealEstatePage />} />
            <Route path="/auction" element={<AuctionPage />} />
            <Route path="/help" element={<HelpPage />} />
            <Route path="/news" element={<NewsPage />} />
            <Route path="/sell" element={<SellPage />} />
          </Route>
          <Route path="/admin" element={<AdminLayout />}>
            <Route index element={<AdminDashboard />} />
            <Route path="auction/ongoing" element={<AuctionOngoing />} />
            <Route path="auction/complete" element={<AuctionComplete />} />
            <Route path="auction/detail/:key" element={<AuctionDetail />} />
            <Route path="user/staff" element={<StaffList />} />
            <Route path="user/member" element={<MemberList />} />
            <Route path="*" element={<PageNotFound />} />
          </Route>
        </Routes>
      </Router>
    </div>
  );
}

export default App;
