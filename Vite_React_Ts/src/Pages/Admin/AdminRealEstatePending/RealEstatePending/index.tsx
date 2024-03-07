import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faArrowLeft} from "@fortawesome/free-solid-svg-icons";
import {
  Table,
  TableProps,
  Tag,
  Descriptions,
  Button,
  notification,
  Form,
  Input,
  InputNumber,
  Select,
  Modal,
} from "antd";
import { useState, useEffect } from "react";
import {
  getRealEstateById,
  getRealEstatePending,
  searchManageRealEstate,
  changeStatusRealPending,
} from "../../../../api/adminRealEstatePending";
import { useContext } from "react";
import { UserContext } from "../../../../context/userContext";
const { TextArea } = Input;

const AdminRealEstatePendingList: React.FC = () => {
  const [search, setSearch] = useState<searchManageRealEstate>({
    reasName: "",
    reasPriceFrom: 0,
    reasPriceTo: 0,
    reasStatus: [1],
  });
  const [RealData, setRealData] = useState<ManageRealEstate[]>(); // State để lưu trữ dữ liệu nhân viên
  const [realDetailData, setRealDetailData] =
    useState<ManageRealEstateDetail>();
  const [showDetail, setShowDetail] = useState<boolean>(false);
  const [ReasId, setReasId] = useState<Number>();
  const [statusReal, setStatusReal] = useState<Number>();
  const [messageReject, getMessageReject] = useState<string>();
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
    handleChangeStatusReject();
  };
  const handleCancel = () => {
    setIsModalOpen(false);
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
        data = await changeStatusRealPending(Id, status, messagestring, token);
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
        if (data?.reasStatus == 0) {
          setStatusReal(1);
        }
        setShowDetail(true);
      }
    } catch (error) {
      console.error("Error fetching real estate detail:", error);
    }
  };

  const fetchReasList = async () => {
    try {
      if (token) {
        let data: ManageRealEstate[] | undefined;
        if (
          search.reasName == "" &&
          search.reasPriceFrom == 0 &&
          search.reasPriceTo == 0
        ) {
          data = await getRealEstatePending(token);
        } else {
          data = await searchManageRealEstate(search, token);
        }
        setRealData(data);
      }
    } catch (error) {
      console.error("Error fetching member list:", error);
    }
  };

  useEffect(() => {
    fetchReasList();
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
  const FormSearch = () => {
    const [form] = Form.useForm(); // Add this line to use form instance

    const onFinish = () => {
      const values = form.getFieldsValue(); // Get all form field values
      setSearch({
        reasName: values.reasName !== undefined ? values.reasName : "",
    reasPriceFrom: values.reasPriceFrom !== undefined ? values.reasPriceFrom : 0,
    reasPriceTo: values.reasPriceTo !== undefined ? values.reasPriceTo : 0,
    reasStatus: [1],
      });
    };


    return (
      <Form
        form={form} // Assign form instance
        name="basic"
        labelCol={{
          span: 8,
        }}
        wrapperCol={{
          span: 16,
        }}
        style={{
          maxWidth: 1200,
          marginTop: 10,
        }}
        onFinish={onFinish} // Assign onFinish handler
        initialValues={search}
      >
        <Form.Item label="Name Reas: " name="reasName">
          <Input style={{ width: 500 }}/>
        </Form.Item>

        <Form.Item label="Price From" name="reasPriceFrom">
          <InputNumber
            addonBefore={selectBefore}
            addonAfter="$"
            defaultValue={""}
            style={{ width: 400 }}
          />
        </Form.Item>

        <Form.Item label="Price To" name="reasPriceTo">
          <InputNumber
            addonBefore={selectBefore}
            addonAfter="$"
            defaultValue={""}
            style={{ width: 400 }}
          />
        </Form.Item>

        <Form.Item
          wrapperCol={{
            offset: 8,
            span: 16,
          }}
        >
          <div
            style={{ display: "flex", justifyContent: "flex-end", width: 900 }}
          >
            <Button htmlType="submit">Search</Button>
          </div>
        </Form.Item>
      </Form>
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
        let color = reas_Status == "In_progress" ? "yellow" : "volcano";
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

  const openNotificationWithIcon = (
    type: "success" | "error",
    description: string
  ) => {
    notification[type]({
      message: "Notification Title",
      description: description,
    });
  };

  const handleBackToList = () => {
    setShowDetail(false);
    fetchReasList();
  };
  const handleChangeStatus = async () => {
    try {
      const response = await fetchChangeStatus(ReasId, statusReal, "");
      if (response !== undefined) {
        // Kiểm tra xem response có được trả về hay không
        if (response.statusCode === "MSG03") {
          openNotificationWithIcon("success", response.message);
        } else {
          openNotificationWithIcon(
            "error",
            "Something went wrong when executing operation. Please try again!"
          );
        }
        setShowDetail(false);
        await fetchReasList();
      }
    } catch (error) {
      console.error("Error handling status change:", error);
    }
  };

  const handleChangeStatusReject = async () => {
    try {
      const response = await fetchChangeStatus(ReasId, 3, messageReject);
      if (response !== undefined) {
        if (response.statusCode === "MSG03") {
          openNotificationWithIcon("success", response.message);
        } else {
          openNotificationWithIcon(
            "error",
            "Something went wrong when executing operation. Please try again!"
          );
        }
        setShowDetail(false);
        await fetchReasList();
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
            <Button onClick={handleChangeStatus}>Approve</Button>

            <Button
              onClick={showModal}
              style={{ marginLeft: "20px", backgroundColor: "red" }}
            >
              Reject
            </Button>
            <Modal
              title="Message Reject"
              open={isModalOpen}
              onOk={handleOk}
              onCancel={handleCancel}
              okButtonProps={{style: { color: "black"}}}
            >
              <TextArea onChange={(e) => getMessageReject(e.target.value)} />
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
          <h4><strong>Real Estate Pending List</strong></h4>
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

export default AdminRealEstatePendingList;
