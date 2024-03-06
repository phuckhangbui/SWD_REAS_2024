import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faArrowLeft, faVolcano } from "@fortawesome/free-solid-svg-icons";
import {
  Table,
  TableProps,
  Tag,
  Descriptions,
  Button,
  notification,
  Input,
  InputNumber,
  Select,
  Modal,
} from "antd";
import { useState, useEffect } from "react";
import {
  changeStatusRealAll,
  getRealEstateAll,
  getRealEstateById,
  searchManageRealEstate,
} from "../../../../api/adminRealEstateAll";
import { useContext } from "react";
import { UserContext } from "../../../../context/userContext";
const { TextArea } = Input;

const AdminRealEstateAllList: React.FC = () => {
  const [search, setSearch] = useState<searchManageRealEstate>({
    reasName: "",
    reasPriceFrom: 0,
    reasPriceTo: 0,
    reasStatus: -1,
  });
  const [RealData, setRealData] = useState<ManageRealEstate[]>(); // State để lưu trữ dữ liệu nhân viên
  const [realDetailData, setRealDetailData] =
    useState<ManageRealEstateDetail>();
  const [showDetail, setShowDetail] = useState<boolean>(false);
  const [message, setMessage] = useState<Message>();
  const [ReasId, setReasId] = useState<Number>();
  const [messageString, getMessageString] = useState<string>();
  const [messageBlock, getMessageBlock] = useState<string>();
  const [isModalOpen, setIsModalOpen] = useState(false);
  const { token } = useContext(UserContext);
  const [showFirstDescription, setShowFirstDescription] = useState(true);
  const [showSecondDescription, setShowSecondDescription] = useState(false);
  const [showThirdDescription, setShowThirdDescription] = useState(false);

  const handleNextDescription = () => {
    if (showFirstDescription) {
      setShowFirstDescription(false);
      setShowSecondDescription(true);
    } else if (showSecondDescription) {
      setShowSecondDescription(false);
      setShowThirdDescription(true);
    }
  };

  const handlePreviousDescription = () => {
    if (showSecondDescription) {
      setShowSecondDescription(false);
      setShowFirstDescription(true);
    } else if (showThirdDescription) {
      setShowThirdDescription(false);
      setShowSecondDescription(true);
    }
  };

  const showModal = () => {
    setIsModalOpen(true);
  };
  const handleOk = () => {
    setIsModalOpen(false);
    handleChangeStatusBlock();
  };
  const handleCancel = () => {
    setIsModalOpen(false);
  };

  const statusColorMap: { [key: string]: string } = {
    InProgress: "green",
    Approved: "green",
    Selling: "orange",
    Cancel: "red",
    Auctioning: "lightgreen",
    Sold: "brown",
    Rollback: "brown",
    ReUp: "blue",
    DeclineAfterAuction: "darkred",
    Block: "lightcoral",
  };

  const formatDate = (dateString: Date): string => {
    const dateObject = new Date(dateString);
    return `${dateObject.getFullYear()}-${(
      "0" +
      (dateObject.getMonth() + 1)
    ).slice(-2)}-${("0" + dateObject.getDate()).slice(-2)} ${(
      "0" + dateObject.getHours()
    ).slice(-2)}:${("0" + dateObject.getMinutes()).slice(-2)}:${(
      "0" + dateObject.getSeconds()
    ).slice(-2)}`;
  };

  const fetchChangeStatus = async (
    Id: Number | undefined,
    status: Number | undefined,
    messagestring: string | undefined
  ) => {
    try {
      if (token) {
        let data: Message | undefined;
        data = await changeStatusRealAll(Id, status, messagestring, token);
        return data;
      }
    } catch (error) {
      console.error("Error fetching change status:", error);
    }
  };

  const fetchRealDetail = async (reasId: Number | undefined) => {
    try {
      if (token) {
        let data: ManageRealEstateDetail | undefined;
        data = await getRealEstateById(reasId, token);
        setRealDetailData(data);
        setReasId(reasId);
        setShowDetail(true);
      }
    } catch (error) {
      console.error("Error fetching real estate detail:", error);
    }
  };

  const fetchReasList = async (searchvalue: any) => {
    try {
      if (token) {
        let data: ManageRealEstate[] | undefined;
        if (
          searchvalue.reasName == "" &&
          searchvalue.reasPriceFrom == 0 &&
          searchvalue.reasPriceTo == 0
        ) {
          data = await getRealEstateAll(token);
          //data = await searchManageRealEstate(search, token);
        } else {
          //data = await getRealEstateAll(token);
          data = await searchManageRealEstate(search, token);
        }
        setRealData(data);
      }
    } catch (error) {
      console.error("Error fetching member list:", error);
    }
  };

  useEffect(() => {
    fetchReasList(search);
  }, [search, token]);

  const viewDetail = (reasId: Number) => {
    fetchRealDetail(reasId);
  };

  const { Option } = Select;
  const selectBefore = (
    <Select
      defaultValue="add"
      style={{
        width: 60,
      }}
    >
      <Option value="add">+</Option>
      <Option value="minus">-</Option>
    </Select>
  );
  const FormSearch = ({}) => {
    const [name, setName] = useState("");
    const [priceFrom, setPriceFrom] = useState(0);
    const [priceTo, setPriceTo] = useState(0);

    const handleSearch = () => {
      setSearch({
        reasName: name,
        reasPriceFrom: priceFrom,
        reasPriceTo: priceTo,
        reasStatus: -1, // Assuming default value for reasStatus
      });
    };

    return (
      <div>
        <Input
          placeholder="Name"
          value={name}
          onChange={(e) => setName(e.target.value)}
        />
        <InputNumber
          addonBefore={selectBefore}
          addonAfter="$"
          defaultValue={priceFrom}
          style={{ width: 400 }}
          onChange={(value) => setPriceFrom(value || 0)}
        />
        <InputNumber
          addonBefore={selectBefore}
          addonAfter="$"
          defaultValue={priceTo}
          style={{ width: 400 }}
          onChange={(value) => setPriceTo(value || 0)}
        />
        <Button onClick={handleSearch}>Search</Button>
      </div>
    );
  };

  const columns: TableProps<ManageRealEstate>["columns"] = [
    {
      title: "Real-Estate Name",
      dataIndex: "reasName",
      width: "20%",
    },
    {
      title: "Price",
      dataIndex: "reasPrice",
      render: (reasPrice: string) => `${reasPrice} $`,
      width: "10%",
    },
    {
      title: "Area",
      dataIndex: "reasArea",
      render: (reasArea: string) => `${reasArea} m²`,
      width: "10%",
    },
    {
      title: "Type",
      dataIndex: "reasTypeName",
      width: "15%",
    },
    {
      title: "Status",
      dataIndex: "reasStatus",
      width: "5%",
      render: (reas_Status: string) => {
        const color = statusColorMap[reas_Status] || "gray"; // Mặc định là màu xám nếu không có trong ánh xạ
        return (
          <Tag color={color} key={reas_Status}>
            {reas_Status.toUpperCase()}
          </Tag>
        );
      },
    },
    {
      title: "Date Start",
      dataIndex: "dateStart",
      width: "15%",
      render: (date_Created: Date) => formatDate(date_Created),
    },
    {
      title: "Date End",
      dataIndex: "dateEnd",
      width: "15%",
      render: (date_End: Date) => formatDate(date_End),
    },

    {
      title: "Actions",
      dataIndex: "operation",
      render: (_: any, reas: ManageRealEstate) => (
        <a onClick={() => viewDetail(reas.reasId)}>View details</a>
      ),
      width: "15%",
    },
  ];

  const renderBorderedItems = () => {
    const items = [
      {
        key: "1",
        label: "Reas Name",
        children: realDetailData?.reasName || "",
      },
      {
        key: "2",
        label: "Price",
        children: realDetailData?.reasPrice + "$" || "",
      },
      {
        key: "3",
        label: "Reas Area",
        children: realDetailData?.reasArea + "m²" || "",
      },
      {
        key: "4",
        label: "Address",
        children: realDetailData?.reasAddress || "",
      },
      {
        key: "5",
        label: "Type",
        children: realDetailData?.type_REAS_Name || "",
      },
      {
        key: "6",
        label: "Status",
        children: realDetailData?.reasStatus,
        render: (reas_Status: Number | undefined) => {
          let color = reas_Status == 0 ? "yellow" : "volcano";
          return (
            <Tag color={color} key={reas_Status?.toString()}>
              {"In_progress"}
            </Tag>
          );
        },
      },
      {
        key: "7",
        label: "Owner",
        children: realDetailData?.accountOwnerName || "",
      },
      {
        key: "8",
        label: "Date Created",
        children: realDetailData ? formatDate(realDetailData.dateStart) : "",
      },
      {
        key: "9",
        label: "Date End",
        children: realDetailData ? formatDate(realDetailData.dateEnd) : "",
      },
      {
        key: "10",
        label: "Description",
        children: realDetailData?.reasDescription ? (
          <div
            dangerouslySetInnerHTML={{ __html: realDetailData.reasDescription }}
          ></div>
        ) : (
          ""
        ),
        span: 3,
      },
    ];
    return items.map((item) => (
      <Descriptions.Item key={item.key} label={item.label}>
        {item.render ? item.render(item.children) : item.children}
      </Descriptions.Item>
    ));
  };

  const renderBorderedItemsPhoto = () => {
    const { photos } = realDetailData || {};
    const itemPhotos = (photos || []).map((photo) => ({
      key: photo.reasPhotoUrl, // Sử dụng reasPhotoUrl làm key
      label: "Photo",
      children: photo.reasPhotoUrl, // Sử dụng reasPhotoUrl làm children
    }));

    return itemPhotos.map((itemPhoto) => (
      <Descriptions.Item key={itemPhoto.key} label={itemPhoto.label}>
        {itemPhoto.children ? (
          <img
            src={itemPhoto.children}
            alt="Photo"
            style={{ maxWidth: "100%", maxHeight: "200px" }}
          />
        ) : (
          "No image available"
        )}
      </Descriptions.Item>
    ));
  };

  const renderBorderedItemsDetailPhoto = () => {
    const items = [
      {
        key: "1",
        label: "Certification Of Land Front Image",
        children: realDetailData?.detail.reas_Cert_Of_Land_Img_Front || "",
      },
      {
        key: "2",
        label: "Certification Of Land After Image",
        children: realDetailData?.detail.reas_Cert_Of_Land_Img_After || "",
      },
      {
        key: "3",
        label: "Certification Of Home's Ownership",
        children: realDetailData?.detail.reas_Cert_Of_Home_Ownership || "",
      },
      {
        key: "4",
        label: "Registration Book",
        children: realDetailData?.detail.reas_Registration_Book || "",
      },
      {
        key: "5",
        label: "Documents Of Proving Marital Relationship",
        children:
          realDetailData?.detail.documents_Proving_Marital_Relationship || "",
      },
      {
        key: "6",
        label: "Sales of Authorization Contract",
        children: realDetailData?.detail.sales_Authorization_Contract || "",
      },
    ];
    return items.map((item) => (
      <Descriptions.Item key={item.key} label={item.label}>
        {item.children ? (
          <img
            src={item.children}
            alt={item.label}
            style={{ maxWidth: "100%", maxHeight: "200px" }}
          />
        ) : (
          "No image available"
        )}
      </Descriptions.Item>
    ));
  };

  const [contextHolder] = notification.useNotification();
  const openNotificationWithIcon = (type: "success" | "error") => {
    notification[type]({
      message: "Notification Title",
      description: message?.message,
    });
  };

  const handleBackToList = () => {
    setShowDetail(false);
    fetchReasList(search);
  };
  const handleChangeStatusActive = async () => {
    try {
      const response = await fetchChangeStatus(ReasId, 2, "");
      if (response !== undefined) {
        // Kiểm tra xem response có được trả về hay không
        setMessage(response);
        if (response.statusCode == "MSG03") {
          getMessageString(response.message);
        } else {
          getMessageString(
            "Something went wrong when executing operation. Please try again!"
          );
        }
        openNotificationWithIcon(
          response.statusCode == "MSG03" ? "success" : "error"
        );
        setShowDetail(false);
        await fetchReasList(search);
      }
    } catch (error) {
      console.error("Error handling status change:", error);
    }
  };

  const handleChangeStatusBlock = async () => {
    try {
      const response = await fetchChangeStatus(ReasId, 9, messageBlock);
      if (response !== undefined) {
        // Kiểm tra xem response có được trả về hay không
        setMessage(response);
        if (response.statusCode == "MSG03") {
          getMessageString(response.message);
        } else {
          getMessageString(
            "Something went wrong when executing operation. Please try again!"
          );
        }
        openNotificationWithIcon(
          response.statusCode == "MSG03" ? "success" : "error"
        );
        setShowDetail(false);
        await fetchReasList(search);
      }
    } catch (error) {
      console.error("Error handling status change:", error);
    }
  };

  return (
    <>
      {showDetail ? (
        <div>
          <Button onClick={handleBackToList}>
            <FontAwesomeIcon icon={faArrowLeft} style={{ color: "#74C0FC" }} />
          </Button>
          <br />
          <br />
          <div style={{ display: "flex", justifyContent: "flex-end" }}>
            {realDetailData?.reasStatus !== 9 && ( // Kiểm tra nếu status không phải "Cancel"
              <Button onClick={handleChangeStatusActive}>Active</Button>
            )}
            {realDetailData?.reasStatus === 9 && ( // Kiểm tra nếu status là "Cancel"
              <Button onClick={handleChangeStatusBlock}>Reject</Button>
            )}
            <Button
              onClick={showModal}
              style={{ marginLeft: "20px", backgroundColor: "red" }}
            >
              Reject
            </Button>
            <Modal
              title="Message Block"
              open={isModalOpen}
              onOk={handleOk}
              onCancel={handleCancel}
              okButtonProps={{ style: { color: "black" } }}
            >
              <TextArea
                onChange={(e: React.ChangeEvent<HTMLTextAreaElement>) =>
                  getMessageBlock(e.target.value)
                }
              />
            </Modal>
          </div>
          <br />
          <div>
            {showFirstDescription && (
              <div>
                <Descriptions bordered title="Detail of Real Estate">
                  {renderBorderedItems()}
                </Descriptions>
                <br /> <br />
                <div style={{ display: "flex", justifyContent: "flex-end" }}>
                  <Button onClick={handleNextDescription}>Next</Button>
                </div>
              </div>
            )}
            {showSecondDescription && (
              <div>
                <Descriptions bordered title="Detail of Real Estate">
                  {renderBorderedItemsPhoto()}
                </Descriptions>
                <br />
                <br />

                <div
                  style={{ display: "flex", justifyContent: "space-between" }}
                >
                  <Button onClick={handlePreviousDescription}>Back</Button>
                  <Button onClick={handleNextDescription}>Next</Button>
                </div>
              </div>
            )}
            {showThirdDescription && (
              <div>
                <Descriptions bordered title="Detail of Real Estate">
                  {renderBorderedItemsDetailPhoto()}
                </Descriptions>
                <br />
                <br />
                <Button onClick={handlePreviousDescription}>Back</Button>
              </div>
            )}
          </div>
        </div>
      ) : (
        <div>
          {/* Bảng danh sách */}
          <div className="flex flex-col items-center justify-between gap-4 md:flex-row">
            <div className="w-full md:w-72 flex flex-row justify-start">
              <FormSearch />
            </div>
          </div>
          <Table columns={columns} dataSource={RealData} bordered />
        </div>
      )}
    </>
  );
};

export default AdminRealEstateAllList;
