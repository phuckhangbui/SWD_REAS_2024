import { Descriptions, Button, Input, notification } from "antd";
import { useState, useEffect, useCallback } from "react";
import { addNews } from "../../../../api/news";
import { useContext } from "react";
import { UserContext } from "../../../../context/userContext";
import CloudinaryUploadWidget, {
  CloudinaryConfig,
  SetPublicIdFunction,
} from "../../../../Config/cloudinary";
import { Cloudinary } from "@cloudinary/url-gen";
import { CKEditor } from "@ckeditor/ckeditor5-react";
import ClassicEditor from "@ckeditor/ckeditor5-build-classic";

const AdminAddNews: React.FC = () => {
  const [newsData, setNewsData] = useState<newscreate>({
    newsContent: "",
    newsSumary: "",
    newsTitle: "",
    thumbnailUri: "",
  });
  const [uploadedImage, setUploadedImage] = useState<string>("");
  const [publicId, setPublicId] = useState<string>("");
  const [uwConfig] = useState<CloudinaryConfig>({
    cloudName: "dqpsvl3nu",
    uploadPreset: "i0yxovxe",
    // cropping: true,
    // showAdvancedOptions: true,
    // sources: [ "local", "url"],
    // multiple: false,
    folder: "News",
    // tags: ["users", "profile"],
    // context: {alt: "user_uploaded"},
    // clientAllowedFormats: ["images"],
    // maxImageFileSize: 2000000,
    // maxImageWidth: 2000,
    // theme: "purple",
  });
  const { token } = useContext(UserContext);

  const fetchCreateNews = async (News: newscreate) => {
    try {
      if (token) {
        let data: Message | undefined;
        data = await addNews(News, token);
        return data;
      }
    } catch (error) {
      console.error("Error fetching add news:", error);
    }
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

  const createNews = async () => {
    const response = await fetchCreateNews(newsData);
    if (response != undefined && response) {
      if (response.statusCode == "MSG21") {
        openNotificationWithIcon("success", response.message);
      } else {
        openNotificationWithIcon(
          "error",
          "Something went wrong when executing operation. Please try again!"
        );
      }
    }
  };

  const handleImageUpload = (imageUrl: string) => {
    setUploadedImage(imageUrl);
    setNewsData((prevData) => ({
      ...prevData,
      thumbnailUri: imageUrl,
    }));
  };

  const handleChange = (fieldName: keyof newscreate, value: string) => {
    setNewsData((prevData) => ({
      ...prevData,
      [fieldName]: value,
    }));
  };
  const renderBorderedItems = () => {
    const items = [
      {
        key: "1",
        label: "Thumbnail",
        children: "",
        span: 3,
        render: (url: string) => (
          <div>
            <img
              className="h-96 w-full object-cover object-center"
              src={uploadedImage}
              alt="Uploaded"
              style={{ maxWidth: "200px", maxHeight: "200px" }}
            />
            <CloudinaryUploadWidget
              uwConfig={uwConfig}
              setPublicId={setPublicId}
              setUploadedUrl={handleImageUpload}
            />
          </div>
        ),
      },
      {
        key: "2",
        label: "Title",
        children: (
          <div>
            <Input
              placeholder="Enter title"
              onChange={(e) => handleChange("newsTitle", e.target.value)}
            />
          </div>
        ),
        span: 3,
      },
      {
        key: "3",
        label: "Summary",
        children: (
          <Input
            placeholder="Enter summary"
            onChange={(e) => handleChange("newsSumary", e.target.value)}
          />
        ),
        span: 3,
      },
      {
        key: "4",
        label: "Content",
        children: (
          <CKEditor
            editor={ClassicEditor}
            onChange={(event, editor) => {
              const data = editor.getData();
              handleChange("newsContent", data);
            }}
          />
        ),
        span: 3,
      },
    ];
    return items.map((item) => (
      <Descriptions.Item key={item.key} label={item.label} span={item.span}>
        {item.render ? item.render(item.children) : item.children}
      </Descriptions.Item>
    ));
  };
  return (
    <>
      <Descriptions bordered title="Add News">
        {renderBorderedItems()}
      </Descriptions>
      <br />
      <div
        style={{
          display: "flex",
          flexDirection: "column",
          alignItems: "center",
        }}
      >
        <Button onClick={createNews}>Create News</Button>
      </div>
    </>
  );
};
export default AdminAddNews;
