import React, { useEffect, useState } from "react";
import { Table } from "antd";
import type { GetProp, TableProps } from "antd";

type ColumnsType<T> = TableProps<T>["columns"];
type TablePaginationConfig = Exclude<
  GetProp<TableProps, "pagination">,
  boolean
>;

interface DataType {
  name: string;
  finalPrice: string;
  ownerName: string;
  dateEnd: any;
}

interface TableParams {
  pagination?: TablePaginationConfig;
  sortField?: string;
  sortOrder?: string;
  filters?: Parameters<GetProp<TableProps, "onChange">>[1];
}

const columns: ColumnsType<DataType> = [
  {
    title: "Real estate",
    dataIndex: "name",
    sorter: true,
  },
  {
    title: "Final bid",
    dataIndex: "finalPrice",
    width: "20%",
  },
  {
    title: "Owner",
    dataIndex: "ownerName",
  },
  {
    title: "Date end",
    dataIndex: "dateEnd",
  },
];

const getRandomuserParams = (params: TableParams) => ({
  results: params.pagination?.pageSize,
  page: params.pagination?.current,
  ...params,
});

const AuctionHistory: React.FC = () => {
  const [data, setData] = useState<DataType[]>();
  const [loading, setLoading] = useState(false);
  const [tableParams, setTableParams] = useState<TableParams>({
    pagination: {
      current: 1,
      pageSize: 10,
    },
  });

  const fetchData = () => {
    setLoading(true);
    // Simulating fetching real estate data
    const fakeRealEstates = Array.from({ length: 10 }, (_, index) => ({
      name: `Real Estate ${index + 1}`,
      finalPrice: `$${Math.floor(Math.random() * 1000000)}`,
      ownerName: `Owner ${index + 1}`,
      dateEnd: new Date(
        Date.now() + Math.floor(Math.random() * 10000000000)
      ).toDateString(), // Random date in the future
    }));
    setData(fakeRealEstates);
    setLoading(false);
    setTableParams({
      ...tableParams,
      pagination: {
        ...tableParams.pagination,
        total: 10, // Assuming there are only 10 real estates for this example
      },
    });
  };

  useEffect(() => {
    fetchData();
  }, [JSON.stringify(tableParams)]);

  const handleTableChange: TableProps["onChange"] = (
    pagination,
    filters,
    sorter
  ) => {
    setTableParams({
      pagination,
      filters,
      ...sorter,
    });

    // `dataSource` is useless since `pageSize` changed
    if (pagination.pageSize !== tableParams.pagination?.pageSize) {
      setData([]);
    }
  };

  return (
    <>
      <div className="pt-20">
        <Table
          columns={columns}
          rowKey={(record) => record.name}
          dataSource={data}
          pagination={tableParams.pagination}
          loading={loading}
          onChange={handleTableChange}
        />
      </div>
    </>
  );
};

export default AuctionHistory;
