import React, { Key, useContext, useEffect, useState } from "react";
import {
  Form,
  FormInstance,
  Input,
  InputNumber,
  Select,
  Table,
  Typography,
} from "antd";
import { AnyObject } from "antd/es/_util/type";
import { NumberFormat } from "../../../../utils/numbetFormat";
import { Avatar, Input as TailwindInput } from "@material-tailwind/react";
import { MagnifyingGlassIcon } from "@heroicons/react/20/solid";

const EditableContext = React.createContext<FormInstance<any> | null>(null);

interface DataType {
  key: number;
  realEstate: string;
  remainingTime: string;
  currentBid: number;
  owner: string;
  img: string;
  auctionType: string;
  bidIncrement: number;
}

interface EditableRowProps {
  index: number;
}

const EditableRow: React.FC<EditableRowProps> = ({ index, ...props }) => {
  const [form] = Form.useForm();
  return (
    <Form form={form} component={false}>
      <EditableContext.Provider value={form}>
        <tr {...props} />
      </EditableContext.Provider>
    </Form>
  );
};

interface EditableCellProps {
  title: React.ReactNode;
  editable: boolean;
  children: React.ReactNode;
  dataIndex: keyof DataType;
  record: DataType;
  handleSave: (record: DataType) => void;
}

const EditableCell: React.FC<EditableCellProps> = ({
  title,
  editable,
  children,
  dataIndex,
  record,
  handleSave,
  ...restProps
}) => {
  const [editing, setEditing] = useState(false);
  // const inputRef = useRef<InputRef>(null);
  const form = useContext(EditableContext)!;

  useEffect(() => {
    if (editing) {
      // inputRef.current!.focus();
    }
  }, [editing]);

  const toggleEdit = () => {
    setEditing(!editing);
    form.setFieldsValue({ [dataIndex]: record[dataIndex] });
  };

  const save = async () => {
    try {
      const values = await form.validateFields();

      toggleEdit();
      handleSave({ ...record, ...values });
    } catch (errInfo) {
      console.log("Save failed:", errInfo);
    }
  };

  let childNode = children;

  if (editable) {
    switch (dataIndex) {
      case "bidIncrement":
        childNode = editing ? (
          <Form.Item
            style={{ margin: 0 }}
            name={dataIndex}
            rules={[
              {
                required: true,
                message: `${title} is required.`,
              },
            ]}
          >
            <InputNumber onPressEnter={save} onBlur={save} />
          </Form.Item>
        ) : (
          <div
            className="editable-cell-value-wrap"
            style={{ paddingRight: 24 }}
            onClick={toggleEdit}
          >
            {children}
          </div>
        );
        break;
      default:
        childNode = editing ? (
          <Form.Item
            style={{ margin: 0 }}
            name={dataIndex}
            rules={[
              {
                required: true,
                message: `${title} is required.`,
              },
            ]}
          >
            <Input onPressEnter={save} onBlur={save} />
          </Form.Item>
        ) : (
          <div
            className="editable-cell-value-wrap"
            style={{ paddingRight: 24 }}
            onClick={toggleEdit}
          >
            {children}
          </div>
        );
        break;
    }
  }

  return <td {...restProps}>{childNode}</td>;
};

type EditableTableProps = Parameters<typeof Table>[0];

type ColumnTypes = Exclude<EditableTableProps["columns"], undefined>;

const OngoingList: React.FC = () => {
  const [form] = Form.useForm();
  const [dataSource, setDataSource] = useState<DataType[]>([]);
  const [search, setSearch] = useState("");
  const [type, setType] = useState("");
  for (let i = 1; i < 100; i++) {
    dataSource.push({
      key: i,
      realEstate: `Property ${i}`,
      remainingTime: "1 hours",
      currentBid: 1000000000 + i,
      owner: `User No. ${i}`,
      img: "https://demos.creative-tim.com/test/corporate-ui-dashboard/assets/img/team-3.jpg",
      auctionType: "department",
      bidIncrement: 10000000 + i,
    });
  }
  const handleSave = (row: DataType) => {
    const newData = [...dataSource];
    const index = newData.findIndex((item) => row.key === item.key);
    const item = newData[index];
    newData.splice(index, 1, {
      ...item,
      ...row,
    });
    setDataSource(newData);
  };

  const handleChange = (value: string) => {
    setType(value);
  };

  const defaultColumns: (ColumnTypes[number] & {
    editable?: boolean;
    dataIndex: string;
  })[] = [
    {
      title: "Id",
      dataIndex: "key",
    },
    {
      title: "Real Estate",
      dataIndex: "realEstate",
    },
    {
      title: "Remaining Time",
      dataIndex: "remainingTime",
    },
    {
      title: "Current Bid",
      dataIndex: "currentBid",
      render: (_: any, record: AnyObject) => NumberFormat(record.currentBid),
      sorter: (a, b) => a.currentBid - b.currentBid,
    },
    {
      title: "Owner",
      dataIndex: "owner",
      render: (_: any, record: AnyObject) => (
        <div className="flex items-center gap-3">
          <Avatar src={record.img} alt={record.owner} size="sm" />
          <div className="flex flex-col">
            <Typography color="blue-gray" className="font-normal">
              {record.owner}
            </Typography>
          </div>
        </div>
      ),
      sorter: (a, b) => a.owner.localeCompare(b.owner),
    },
    {
      title: "Auction Type",
      dataIndex: "auctionType",
      filters: [
        { value: "residential", text: "Residential" },
        { value: "commercial", text: "Commercial" },
        { value: "land", text: "Land" },
        { value: "investment", text: "Investment" },
        { value: "farmland", text: "Farmland" },
        { value: "specialPurpose", text: "Special Purpose" },
        { value: "department", text: "Department" },
      ],
      onFilter: (value: boolean | Key, record: AnyObject) =>
        record.auctionType.indexOf(String(value)) === 0,
    },
    {
      title: "Bid Increment",
      dataIndex: "bidIncrement",
      render: (_: any, record: AnyObject) => NumberFormat(record.bidIncrement),
      editable: true,
      sorter: (a, b) => a.bidIncrement - b.bidIncrement,
    },
    {
      dataIndex: "operation",
      render: (_: any) => <a>View details</a>,
    },
  ];

  const components = {
    body: {
      row: EditableRow,
      cell: EditableCell,
    },
  };

  const columns = defaultColumns.map((col) => {
    if (!col.editable) {
      return col;
    }
    return {
      ...col,
      onCell: (record: DataType) => ({
        record,
        editable: col.editable,
        dataIndex: col.dataIndex,
        title: col.title,
        handleSave,
      }),
    };
  });
  return (
    <>
      <div className="flex flex-col items-center justify-between gap-4 md:flex-row">
        <div className="w-full md:w-72 flex flex-row justify-start">
          <Select
            defaultValue="Residential"
            onChange={handleChange}
            options={[
              { value: "residential", label: "Residential" },
              { value: "commercial", label: "Commercial" },
              { value: "land", label: "Land" },
              { value: "investment", label: "Investment" },
              { value: "farmland", label: "Farmland" },
              { value: "specialPurpose", label: "Special Purpose" },
              { value: "department", label: "Department" },
            ]}
          />
        </div>

        <div className="w-full md:w-72 flex flex-row justify-end">
          <div className="flex flex-row space-between space-x-2">
            <TailwindInput
              label="Search"
              icon={<MagnifyingGlassIcon className="h-5 w-5" />}
              crossOrigin={undefined}
              onChange={(e) => setSearch(e.target.value)}
            />
          </div>
        </div>
      </div>
      <Form form={form} component={false}>
        <Table
          components={components}
          rowClassName={() => "editable-row"}
          bordered
          dataSource={dataSource.filter((reas) => {
            const isMatchingSearch =
              search.toLowerCase() === "" ||
              reas.realEstate.toLowerCase().includes(search);

            const filterType =
              type.toLocaleLowerCase() === "" ||
              reas.auctionType.toLocaleLowerCase().includes(type);

            return isMatchingSearch && filterType;
          })}
          columns={columns as ColumnTypes}
        />
      </Form>
    </>
  );
};

export default OngoingList;
