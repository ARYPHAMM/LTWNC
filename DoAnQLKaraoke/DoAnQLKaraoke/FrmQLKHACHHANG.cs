﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using DoAnQLKaraokeBUS;
using DoAnQLKaraokeDTO;

namespace DoAnQLKaraoke
{
    public partial class FrmQLKHACHHANG : Form
    {
        public int trThai = 1;
        KhachHangDTO KHhienhanh;
        string maKHMoi = null;
        public FrmQLKHACHHANG()
        {
            InitializeComponent();
            dgv_KhachHang.AutoGenerateColumns = false;
        }
        private void FrmQLKHACHHANG_Load(object sender, EventArgs e)
        {
            LoadData();

            txt_MaKH.Enabled = false;
            TrangThai();
        }
        private void TrangThai()
        {
            switch (trThai)
            {
                case 1: // mac dinh
                    {
                        //pic_hinh.BackgroundImage = Properties.Resources.s22;
                        btn_luu.Text = "Lưu";
                        btn_luu.Image = Properties.Resources.save;
                        btn_them.Enabled = true;
                        btn_them.Text = "Thêm";
                        btn_capNhat.Text = "Cập nhật";
                        btn_them.Image = Properties.Resources.add;
                        btn_capNhat.Image = Properties.Resources.edit;
                        btn_luu.Visible = false;
                        txt_MaKH.Enabled = false;

                        txt_Ten.Enabled = false;
                        cbo_LoaiKH.Enabled = true;
                        txt_SDT.Enabled = false;
                        cbo_TinhTrang.Enabled = true;
                        KHhienhanh = null;
                        //Bind();

                    }
                    break;
                case 2: // them moi
                    {
                        btn_them.Enabled = true;
                        btn_them.Text = "Hủy";
                        btn_them.Image = Properties.Resources.cancel;
                        btn_capNhat.Enabled = false;
                        btn_luu.Visible = true;
                        txt_Ten.Enabled = true;

                        cbo_LoaiKH.Enabled = true;
                        txt_SDT.Enabled = true;
                        cbo_TinhTrang.Enabled = true;
                        KHhienhanh = null;
                        //Bind();
                    }
                    break;
                case 3: // chinh sua
                    {
                        btn_them.Enabled = false;
                        btn_luu.Visible = true;
                        btn_capNhat.Enabled = true;
                        btn_capNhat.Text = "Hủy";
                        btn_capNhat.Image = Properties.Resources.cancel;
                        btn_luu.Visible = true;

                        txt_Ten.Enabled = true;
                        txt_SDT.Enabled = true;
                        cbo_LoaiKH.Enabled = true;
                        cbo_TinhTrang.Enabled = true;

                    }
                    break;
            }
        }
        public void LoadData()
        {
            KhachHangBUS a = new KhachHangBUS();
            maKHMoi = a.MaKHMoi();
            txt_MaKH.Text = maKHMoi;
            //trThai = 1;
            //TrangThai();
            LoaiKhachHangBUS lp = new LoaiKhachHangBUS();
            cbo_LoaiKH.DataSource = lp.DanhSachLoaiKH().FindAll(o => o.TINHTRANG == true);
            cbo_LoaiKH.ValueMember = "MALOAI";
            cbo_LoaiKH.DisplayMember = "TENLOAIKH";

            TinhTrangKHBUS tt = new TinhTrangKHBUS();
            cbo_TinhTrang.DataSource = tt.DSTINHTRANGKH();
            cbo_TinhTrang.ValueMember = "MATTR";
            cbo_TinhTrang.DisplayMember = "TENTTR";

            //load datagridviewcomboboxcolum truoc khi load ds .
            DataGridViewComboBoxColumn dgvcb_loaiphong = (DataGridViewComboBoxColumn)dgv_KhachHang.Columns["colLOAI"];
            dgvcb_loaiphong.DataSource = lp.DanhSachLoaiKH().FindAll(o => o.TINHTRANG == true);
            dgvcb_loaiphong.ValueMember = "MALOAI";
            dgvcb_loaiphong.DisplayMember = "TENLOAIKH";

            DataGridViewComboBoxColumn dgvcb_tinhtrang = (DataGridViewComboBoxColumn)dgv_KhachHang.Columns["colTT"];
            dgvcb_tinhtrang.DataSource = tt.DSTINHTRANGKH();
            dgvcb_tinhtrang.ValueMember = "MATTR";
            dgvcb_tinhtrang.DisplayMember = "TENTTR";

            //DataGridViewCheckBoxColumn dgvckb = (DataGridViewCheckBoxColumn)dgv_sanpham.Columns["TinhTrang"];
            KhachHangBUS b = new KhachHangBUS();
            dgv_KhachHang.DataSource = b.DanhSachKhachHang();


        }
        private void Bind()
        {


            if (KHhienhanh != null)
            {
                txt_MaKH.Text = KHhienhanh.MAKH;

                txt_Ten.Text = KHhienhanh.TENKH;
                txt_SDT.Text = KHhienhanh.SDT;
                cbo_LoaiKH.SelectedValue = KHhienhanh.LOAIKH;
                cbo_TinhTrang.SelectedValue = KHhienhanh.TINHTRANG;

            }
            else
            {
                if (dgv_KhachHang.SelectedRows.Count == 0)
                {
                    //txt_MaKH.Text = maKHMoi; // truong hop dang cap nhat
                }



                txt_Ten.Text = string.Empty;
                txt_SDT.Text = string.Empty;
                cbo_LoaiKH.SelectedIndex = -1;
                cbo_TinhTrang.SelectedIndex = -1;
            }
        }


        private void dgv_KhachHang_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv_KhachHang.SelectedRows.Count > 0)
            {
                trThai = 1;
                TrangThai();
                btn_capNhat.Enabled = true;
                KHhienhanh = (KhachHangDTO)dgv_KhachHang.SelectedRows[0].DataBoundItem; // dgvSinhVien.CurrentRow
            }
            else
            {
                btn_capNhat.Enabled = false;
                KHhienhanh = null;
            }
            Bind();
        }

        private void btn_luu_Click(object sender, EventArgs e)
        {
            KhachHangDTO khachhang = new KhachHangDTO();
            try
            {

                khachhang.MAKH = txt_MaKH.Text.Trim();
                khachhang.TENKH = txt_Ten.Text.Trim();
                khachhang.SDT = txt_SDT.Text.Trim();
                khachhang.LOAIKH = int.Parse(cbo_LoaiKH.SelectedValue.ToString());
                khachhang.TINHTRANG = int.Parse(cbo_TinhTrang.SelectedValue.ToString());

            }
            catch
            {
                MessageBox.Show("Thieu thong tin");
                return;

            }

            KhachHangBUS a = new KhachHangBUS();
            if (trThai == 2)
            {



                bool kq = a.ThemKH(khachhang);
                if (kq)
                {
                    //MessageBox.Show("Them thanh cong", maKHMoi);
                    trThai = 1;
                }
                else
                    MessageBox.Show("Them that bai !");


            }
            else
            {
                bool kt = a.CapNhatKH(khachhang);
                if (!kt)
                {
                    MessageBox.Show("Cập nhật thất bại");
                }
                else
                {
                    MessageBox.Show("Cập nhật thành công!");
                }

                trThai = 1;
            }
            LoadData();
            KHhienhanh = null;
            TrangThai();
            Bind();
  

        }

        private void btn_lamMoi_Click_1(object sender, EventArgs e)
        {
            KHhienhanh = null;
            Bind();
        }

        private void btn_them_Click_1(object sender, EventArgs e)
        {
            if (trThai == 2) // lan kich thu 2
            {
                trThai = 1;
                TrangThai();

            }
            else
            {
                trThai = 2;
                TrangThai();
            }
        }

        private void btn_capNhat_Click_1(object sender, EventArgs e)
        {
            if (trThai == 3) // lan kich thu 2
            {
                trThai = 1;
                TrangThai();

            }
            else
            {
                trThai = 3;
                TrangThai();
            }
        }
        private void XuLyTraCuu(int loai, int tinhTrang)
        {
            KhachHangBUS b = new KhachHangBUS();
            List<KhachHangDTO> lst = b.DanhSachKhachHang().FindAll(o => o.LOAIKH == loai && o.TINHTRANG == tinhTrang);
            dgv_KhachHang.DataSource = lst;
        }
        private void btn_traCuu_Click_1(object sender, EventArgs e)
        {
            try
            {
                XuLyTraCuu(int.Parse(cbo_LoaiKH.SelectedValue.ToString()), int.Parse(cbo_TinhTrang.SelectedValue.ToString()));
            }
            catch
            {

            }
            btn_luu.Enabled = false;
        }

        private void btn_qlLoaiKH_Click_1(object sender, EventArgs e)
        {
            this.Visible = false;
            FrmQLLoaiKhachHang ql = new FrmQLLoaiKhachHang();
            ql.Show();
        }

        private void btn_qlLoaiKH_Click_2(object sender, EventArgs e)
        {

        }
    }
}
