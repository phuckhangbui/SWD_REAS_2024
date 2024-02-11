import React, { useState } from "react";
import {
  DesktopOutlined,
  FileOutlined,
  PieChartOutlined,
  TeamOutlined,
  UserOutlined,
  MenuUnfoldOutlined,
  MenuFoldOutlined,
} from "@ant-design/icons";
import type { MenuProps } from "antd";
import { Button, Col, Layout, Menu, Row, theme } from "antd";
import { Outlet, useNavigate } from "react-router-dom";

const { Header, Content, Footer, Sider } = Layout;

type MenuItem = Required<MenuProps>["items"][number];

function getItem(
  label: React.ReactNode,
  key: React.Key,
  icon?: React.ReactNode,
  children?: MenuItem[]
): MenuItem {
  return {
    key,
    icon,
    children,
    label,
  } as MenuItem;
}

const items: MenuItem[] = [
  getItem("Dashboard", "1", <PieChartOutlined />),
  getItem("Auction", "sub1", <DesktopOutlined />, [
    getItem("Ongoing Auctions", "2"),
    getItem("Completed Auctions", "3"),
  ]),
  getItem("User", "sub2", <UserOutlined />, [
    getItem("Staffs", "4"),
    getItem("Members", "5"),
  ]),
  getItem("Real Estate", "sub3", <TeamOutlined />, [
    getItem("Confirmed", "6"),
    getItem("Pending Confirmation", "7"),
  ]),
  getItem("Reporting and Statistics", "8", <FileOutlined />),
  getItem("System setting", "9", <FileOutlined />),
];

const Sidebar: React.FC = () => {
  const [collapsed, setCollapsed] = useState(false);
  const {
    token: { colorBgContainer },
  } = theme.useToken();

  const navigate = useNavigate();

  const handleMenuClick = (key: React.Key) => {
    switch (key) {
      case "1":
        navigate("/admin");
        break;
      case "2":
        navigate("/admin/auction/ongoing");
        break;
      case "3":
        navigate("/admin/auction/complete");
        break;
      case "4":
        navigate("/admin/user/staff");
        break;
      case "5":
        navigate("/admin/user/member");
        break;
      case "6":
        navigate("/admin/real-estate/confirmed");
        break;
      case "7":
        navigate("/admin/real-estate/pending");
        break;
      case "8":
        navigate("/admin/reporting-statistics");
        break;
      case "9":
        navigate("/admin/system-setting");
        break;
      default:
        break;
    }
  };
  return (
    <Layout style={{ minHeight: "100vh" }}>
      <Sider collapsed={collapsed} onCollapse={(value) => setCollapsed(value)}>
        <div className="demo-logo-vertical" />
        <Menu
          theme="dark"
          defaultSelectedKeys={["1"]}
          mode="inline"
          items={items}
          onClick={({ key }) => handleMenuClick(key)}
        />
      </Sider>
      <Layout>
        <Header style={{ padding: 0, background: colorBgContainer }}>
          <Button
            type="text"
            icon={collapsed ? <MenuUnfoldOutlined /> : <MenuFoldOutlined />}
            onClick={() => setCollapsed(!collapsed)}
            style={{
              fontSize: "16px",
              width: 64,
              height: 64,
            }}
          />
        </Header>
        <Content className="w-full">
          <Row
            gutter={[16, 16]}
            style={{
              margin: 0,
            }}
          >
            <Col span={24}>
              <main>
                <Outlet />
              </main>
            </Col>
          </Row>
        </Content>
        <Footer style={{ textAlign: "center" }}>
          Ant Design ©{new Date().getFullYear()} Created by Ant UED
        </Footer>
      </Layout>
    </Layout>
  );
};

export default Sidebar;
