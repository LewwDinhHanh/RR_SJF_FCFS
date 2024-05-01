using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS
{
    class FCFS_alg
    {
        //inputs
		public int num_process; // số lượng quá trình
        public int[] cpu_brustTime; // mảng chứa thời gian xử lý CPU cho mỗi quá trình
        public int[] arrivalTime; // mảng chứa thời gian đến của mỗi quá trình
        public int[] priority; // mảng chứa ưu tiên của mỗi quá trình

		//outputs
        public int[] waitingTime; // Mảng chứa thời gian chờ của mỗi quá trình
        public int[] turnaroundTime; // mảng chứa thời gian hoàn thành của mỗi quá trình
        public int[] start; // mảng chứa thời gian bắt đầu xử lý của mỗi quá trình
        public int[] end; // mảng chứa thời gian kết thúc xử lý của mỗi quá trình
        public int[] proc; // mảng chứa chỉ số của quá trình
        public float avg_waiting; // giá trị trung bình của thời gian chờ 
        public float avg_turnaround; // giá trị trung bình của thời gian hoàn thành
        public int lastValid; // chỉ số cuối cùng của quá trình hợp lệ trong mảng start, end, proc

        public FCFS_alg(int[] a, int[] b, int[] c, int num)
        {
            num_process = num; // số lượng quá trình
            arrivalTime = a; // mảng chứa thời gian đến
            cpu_brustTime = b; // mảng chứa thời gian xử lý cpu
            priority = c; // mảng chứa ưu tiên
            waitingTime = new int[2000]; //Khởi tạo mảng waitingTime với kích thước 2000, lưu trữ thời gian chờ của mỗi quá trình.
            turnaroundTime = new int[2000]; //Khởi tạo mảng turnaroundTime với kích thước 2000, lưu trữ thời gian hoàn thành của mỗi quá trình.
            start = new int[2000]; //Khởi tạo mảng start với kích thước 2000, lưu trữ thời gian bắt đầu xử lý của mỗi quá trình.
            end = new int[2000]; //Khởi tạo mảng end với kích thước 2000, lưu trữ thời gian kết thúc xử lý của mỗi quá trình.
            proc = new int[2000]; //Khởi tạo mảng proc với kích thước 2000, lưu trữ chỉ số của quá trình.
            // khởi tạo giá trị ban đầu cho các mảng = 0
            for (int i = 0; i < 2000; i++)
            {
                waitingTime[i] = 0; 
                turnaroundTime[i] = 0; 
                start[i] = 0; 
                end[i] = 0; 
                proc[i] = -1;
            }

        }

        // Xóa dữ liệu đã có trong các biến và mảng chuẩn bị cho việc sử dụng lại lớp đó và dữ liệu mới
        public void clearData()
        {
            arrivalTime = new int[0]; 
            cpu_brustTime = new int[0]; 
            priority = new int[0];
            for (int i = 0; i < 2000; i++)
            {
                waitingTime[i] = 0; turnaroundTime[i] = 0; start[i] = 0; end[i] = 0; proc[i] = -1;
            }
        }
        public void computeFCFS()
        {
            List<process> p = new List<process>(num_process);
            //Tạo một danh sách rỗng có sức chứa ban đầu là num_process để lưu trữ các đối tượng quá trình.

            for (int i = 0; i < num_process; i++)
            {
                p.Add (new process(i, arrivalTime[i], cpu_brustTime[i]));
            }

            process.sort1(p);
            // Gọi phương thức sort1 (giả định rằng đã được định nghĩa trong lớp process) để sắp xếp danh sách p theo thứ tự tăng dần của thời gian đến (arrival) của các quá trình.

            int cnt = 0,idx = 0; // cnt: thời gian đang xét, idx: chỉ số của quá trình
            for(int i = 0; i < num_process; i++)
            {
                if (p[i].arrival <= cnt)
                {
                    start[idx] = cnt;
                    end[idx] = cnt + p[i].burst_time;
                    proc[idx] = p[i].index;
                    waitingTime[i] = start[idx] - p[i].arrival;
                    turnaroundTime[i] = end[idx] - p[i].arrival;
                    cnt += p[i].burst_time;
                    idx++;
                }
                else
                {
                    start[idx] = cnt;
                    end[idx] = p[i].arrival;
                    proc[idx] = -1;
                    cnt = p[i].arrival;
                    idx++;
                    start[idx] = cnt;
                    end[idx] = cnt + p[i].burst_time;
                    proc[idx] = p[i].index;
                    waitingTime[i] = start[idx] - p[i].arrival;
                    turnaroundTime[i] = end[idx] - p[i].arrival;
                    cnt += p[i].burst_time;
                    idx++;
                }
            }
            for(int i = 0; i <num_process; i++)
            {
                if (waitingTime[i] == 0) continue;
                else
                {
                    avg_waiting += waitingTime[i];
                }
            }
            avg_waiting /= num_process;
            for (int i = 0; i < num_process; i++)
            {
                if (turnaroundTime[i] == 0) continue;
                else
                {
                    avg_turnaround += turnaroundTime[i];
                }
            }
            avg_turnaround /= num_process;
            lastValid = idx;
        }
    }
}
