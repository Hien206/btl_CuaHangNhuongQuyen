import zipfile, os

doc_xml_template = '''<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<w:document xmlns:wpc="http://schemas.microsoft.com/office/word/2010/wordprocessingCanvas" xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006" xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:r="http://schemas.openxmlformats.org/officeDocument/2006/relationships" xmlns:m="http://schemas.openxmlformats.org/officeDocument/2006/math" xmlns:v="urn:schemas-microsoft-com:vml" xmlns:wp14="http://schemas.microsoft.com/office/word/2010/wordprocessingDrawing" xmlns:wp="http://schemas.openxmlformats.org/drawingml/2006/wordprocessingDrawing" xmlns:w10="urn:schemas-microsoft-com:office:word" xmlns:w="http://schemas.openxmlformats.org/wordprocessingml/2006/main" xmlns:w14="http://schemas.microsoft.com/office/word/2010/wordml" xmlns:w15="http://schemas.microsoft.com/office/word/2012/wordml" xmlns:wpg="http://schemas.microsoft.com/office/word/2010/wordprocessingGroup" xmlns:wpi="http://schemas.microsoft.com/office/word/2010/wordprocessingInk" xmlns:wne="http://schemas.microsoft.com/office/word/2006/wordml" xmlns:wps="http://schemas.microsoft.com/office/word/2010/wordprocessingShape" mc:Ignorable="w14 w15 wp14">
<w:body>
{body_content}
<w:sectPr>
  <w:pgSz w:w="11906" w:h="16838"/>
  <w:pgMar w:top="1440" w:right="1440" w:bottom="1440" w:left="1700" w:header="720" w:footer="720" w:gutter="0"/>
</w:sectPr>
</w:body>
</w:document>'''

from xml.sax.saxutils import escape

def p_title(text):
    text = escape(text)
    return f'''<w:p>
      <w:pPr>
        <w:jc w:val="center"/>
        <w:spacing w:before="240" w:after="240"/>
      </w:pPr>
      <w:r>
        <w:rPr>
          <w:rFonts w:ascii="Times New Roman" w:hAnsi="Times New Roman"/>
          <w:b/>
          <w:sz w:val="32"/>
          <w:szCs w:val="32"/>
        </w:rPr>
        <w:t>{text}</w:t>
      </w:r>
    </w:p>'''

def p_heading(text, level=1):
    text = escape(text)
    sz = 28 if level == 1 else (26 if level == 2 else 24)
    space_before = 180 if level == 1 else 120
    space_after = 60
    return f'''<w:p>
      <w:pPr>
        <w:spacing w:before="{space_before}" w:after="{space_after}"/>
      </w:pPr>
      <w:r>
        <w:rPr>
          <w:rFonts w:ascii="Times New Roman" w:hAnsi="Times New Roman"/>
          <w:b/>
          <w:sz w:val="{sz}"/>
          <w:szCs w:val="{sz}"/>
        </w:rPr>
        <w:t>{text}</w:t>
      </w:r>
    </w:p>'''

def p_body(text, indent=True, bold_prefix=None):
    text = escape(text)
    ind_xml = '<w:ind w:firstLine="720"/>' if indent else ''
    prefix_xml = ''
    if bold_prefix:
        b_text = escape(bold_prefix)
        prefix_xml = f'''<w:r>
          <w:rPr>
            <w:rFonts w:ascii="Times New Roman" w:hAnsi="Times New Roman"/>
            <w:b/>
            <w:sz w:val="26"/>
            <w:szCs w:val="26"/>
          </w:rPr>
          <w:t>{b_text}</w:t>
        </w:r>'''
    return f'''<w:p>
      <w:pPr>
        <w:jc w:val="both"/>
        <w:spacing w:line="276" w:lineRule="auto" w:after="100"/>
        {ind_xml}
      </w:pPr>
      {prefix_xml}
      <w:r>
        <w:rPr>
          <w:rFonts w:ascii="Times New Roman" w:hAnsi="Times New Roman"/>
          <w:sz w:val="26"/>
          <w:szCs w:val="26"/>
        </w:rPr>
        <w:t>{text}</w:t>
      </w:r>
    </w:p>'''

content = []
content.append(p_title('CHƯƠNG 1: TỔNG QUAN VỀ ĐỀ TÀI'))

content.append(p_heading('1.1. Lý do chọn đề tài', 1))
content.append(p_body('Trong xu thế hội nhập kinh tế và thương mại hóa hiện nay, mô hình kinh doanh nhượng quyền thương mại (Franchise) đang trở thành một trong những phương thức mở rộng quy mô kinh doanh hiệu quả và phổ biến hàng đầu tại Việt Nam và trên thế giới. Mô hình này xuất hiện mạnh mẽ trong các ngành thực phẩm &amp; đồ uống (F&B), bán lẻ, thời trang, và dịch vụ tiện ích. Bằng việc cho phép bên nhận quyền sử dụng thương hiệu, công thức và quy trình vận hành chuẩn mực từ bên nhượng quyền, doanh nghiệp chủ quản có thể nhanh chóng nhân rộng độ phủ thị trường mà không cần trực tiếp gánh chịu toàn bộ chi phí đầu tư mặt bằng và nhân sự.'))
content.append(p_body('Tuy nhiên, việc gia tăng nhanh chóng số lượng các cửa hàng chi nhánh cũng đặt ra bài toán vô cùng phức tạp về công tác quản trị và giám sát hoạt động. Trên thực tế, nhiều chuỗi cửa hàng hiện nay vẫn đang vận hành dựa trên các phương thức thủ công hoặc bán tự động:'))
content.append(p_body('Việc báo cáo doanh số mỗi ngày chủ yếu thông qua file Excel gửi qua email, tin nhắn mạng xã hội, dẫn đến tình trạng chậm trễ, sai lệch dữ liệu và thiếu tính đồng bộ tức thời.', True, '• Ghi nhận doanh thu rời rạc: '))
content.append(p_body('Mỗi hợp đồng nhượng quyền có thể quy định tỷ lệ trích nộp phí (Royalty Fee) khác nhau, thời hạn hiệu lực khác nhau. Việc tổng hợp doanh số cả tháng để tính tiền phí, thuế VAT và xuất hóa đơn thủ công dễ gây nhầm lẫn, tranh chấp giữa thương hiệu và chủ cửa hàng.', True, '• Khó khăn trong tính toán và thu phí nhượng quyền: '))
content.append(p_body('Doanh nghiệp chủ quản khó nắm bắt kịp thời các chi nhánh cố tình nộp chậm báo cáo doanh thu hoặc các hợp đồng sắp đến ngày đáo hạn để thực hiện tái ký hoặc gia hạn hợp đồng.', True, '• Thiếu cơ chế cảnh báo tự động: '))
content.append(p_body('Hóa đơn thu phí hay các phụ lục hợp đồng chưa được tự động số hóa xuất ra định dạng PDF hay bảng tính Excel có cấu trúc bảo mật.', True, '• Lưu trữ và xuất hồ sơ thiếu chuyên nghiệp: '))
content.append(p_body('Trước những thách thức trên, việc ứng dụng kiến trúc phát triển phần mềm hướng dịch vụ (Service-Oriented Architecture - SOA / RESTful API) nhằm xây dựng một hệ thống quản lý tập trung, hiện đại và bảo mật là hết sức cấp thiết. Hệ thống này không chỉ giúp tự động hóa quy trình quản lý cửa hàng, hợp đồng, tự động tính toán phí nhượng quyền, cảnh báo tiến trình ngầm mà còn cung cấp giao diện trực quan hỗ trợ các bên phối hợp vận hành nhịp nhàng.'))
content.append(p_body('Xuất phát từ yêu cầu thực tiễn đó, cùng với kiến thức tiếp thu được từ môn học "Phát triển phần mềm hướng dịch vụ", em đã lựa chọn đề tài: "Quản lý chuỗi cửa hàng nhượng quyền (Franchise)" làm nội dung nghiên cứu và thực hiện bài tập lớn.'))

content.append(p_heading('1.2. Mục tiêu của đề tài', 1))
content.append(p_heading('1.2.1 Mục tiêu tổng quát', 2))
content.append(p_body('Nghiên cứu, phân tích, thiết kế và xây dựng hoàn chỉnh hệ thống phần mềm hướng dịch vụ (Web API) phục vụ công tác quản lý chuỗi cửa hàng nhượng quyền thương mại. Hệ thống đóng vai trò làm trung tâm điều phối dữ liệu xuyên suốt giữa Ban quản trị thương hiệu, Kế toán hệ thống và các Chủ cửa hàng chi nhánh; đảm bảo tính minh bạch, chính xác trong ghi nhận doanh số, tự động hóa tính phí nhượng quyền và kiểm soát rủi ro vận hành.'))

content.append(p_heading('1.2.2 Mục tiêu cụ thể', 2))
content.append(p_body('Để hiện thực hóa mục tiêu tổng quát, đề tài hướng tới các mục tiêu cụ thể sau:'))
content.append(p_body('Xây dựng hệ thống Backend Web API theo mô hình phân tầng 3 lớp (3-Tier Clean Architecture) trên nền tảng .NET 8 (C#). Cài đặt cơ chế kiểm soát truy cập và xác thực danh tính người dùng bằng chuỗi JSON Web Token (JWT) theo mô hình phân quyền dựa trên vai trò (RBAC) gồm 3 đối tượng: Admin, Chủ cửa hàng, Kế toán.', True, 'a) Về kiến trúc dịch vụ và công nghệ Backend: '))
content.append(p_body('Quản lý hồ sơ cơ sở nhượng quyền với cơ chế xóa mềm (Soft-delete); quản lý hợp đồng có theo dõi phiên bản (versioning) và tỷ lệ % phí; ghi nhận doanh số theo ngày có ràng buộc chống trùng lặp; tự động tính phí nhượng quyền và thuế VAT 10%; tích hợp tiến trình ngầm (Hosted Background Service) tự động quét cảnh báo nộp chậm doanh thu và hợp đồng sắp hết hạn; hỗ trợ xuất hóa đơn PDF qua QuestPDF và báo cáo doanh thu Excel qua ClosedXML.', True, 'b) Về chức năng nghiệp vụ cốt lõi: '))
content.append(p_body('Xây dựng giao diện Web responsive thân thiện bằng HTML5, CSS3, JavaScript và Bootstrap 5, giao tiếp bất đồng bộ với Backend API thông qua các lời gọi fetch().', True, 'c) Về giao diện người dùng (Frontend): '))

content.append(p_heading('1.3. Giới hạn và phạm vi của đề tài', 1))
content.append(p_heading('1.3.1 Đối tượng nghiên cứu', 2))
content.append(p_body('Quy trình quản lý chuỗi nhượng quyền thương mại (Franchise); kiến trúc phát triển ứng dụng hướng dịch vụ RESTful API; công nghệ .NET 8, hệ quản trị cơ sở dữ liệu Microsoft SQL Server; các kỹ thuật bảo mật JWT, xử lý tác vụ ngầm Background Service, và thư viện sinh báo cáo PDF/Excel.', True, '• Đối tượng nghiên cứu: '))
content.append(p_body('Gồm Ban Quản trị viên (Admin) quản lý danh mục và duyệt hợp đồng; Chủ cửa hàng (Franchisee) báo cáo doanh số ngày và nhận cảnh báo; Kế toán (Accountant) lập hóa đơn thu phí và xuất báo cáo tài chính.', True, '• Khách thể nghiên cứu: '))

content.append(p_heading('1.3.2 Phạm vi nghiên cứu', 2))
content.append(p_body('Ứng dụng tập trung vào quy trình tương tác giữa trụ sở chính (Franchisor) và các cơ sở nhượng quyền (Franchisee). Đề tài tập trung vào tầng tổng hợp dữ liệu doanh số ngày, tính toán phí nhượng quyền và kiểm soát hợp đồng.', True, '• Phạm vi không gian (Nghiệp vụ): '))
content.append(p_body('Nghiên cứu và hiện thực hóa trên nền tảng .NET 8 Web API, C#, SQL Server kết hợp HTML5/Bootstrap 5; kiểm thử API bằng Swagger và Postman.', True, '• Phạm vi công nghệ: '))
content.append(p_body('Về khoa học, làm sáng tỏ phương pháp thiết kế phần mềm hướng dịch vụ phân tầng chuẩn mực, vận dụng các nguyên lý SOLID, Dependency Injection. Về thực tiễn, đưa ra giải pháp số hóa khả thi giúp tối ưu hóa chi phí vận hành cho các chuỗi nhượng quyền vừa và nhỏ.', True, '• Ý nghĩa khoa học và thực tiễn: '))

content.append(p_heading('1.4. Nội dung thực hiện', 1))
content.append(p_body('1. Khảo sát và thu thập yêu cầu: Tìm hiểu các quy định, biểu mẫu hợp đồng và cách thức thu phí trong mô hình kinh doanh nhượng quyền thương mại thực tế.'))
content.append(p_body('2. Phân tích và thiết kế hệ thống: Xây dựng sơ đồ thực thể liên kết (ERD) với 10 bảng dữ liệu chuẩn hóa; phân tích ca sử dụng (Use Case) và luồng dữ liệu.'))
content.append(p_body('3. Hiện thực hóa tầng dữ liệu và nghiệp vụ (DAL & BLL): Xây dựng DatabaseHelper ADO.NET parameterized an toàn; cài đặt logic tính phí, versioning hợp đồng, audit log và soft-delete.'))
content.append(p_body('4. Phát triển Web API và các dịch vụ bổ trợ: Xây dựng Controller RESTful, Middleware JWT, chính sách CORS, tiến trình ngầm HopDongExpirationBackgroundService, xuất file QuestPDF và ClosedXML.'))
content.append(p_body('5. Xây dựng giao diện Frontend: Thiết kế giao diện Dashboard quản trị và trang đăng nhập bằng HTML, CSS, JavaScript thuần.'))
content.append(p_body('6. Kiểm thử và đánh giá: Xây dựng kịch bản kiểm thử End-to-End (E2E) trên Postman và Swagger UI; hiệu chỉnh lỗi.'))
content.append(p_body('7. Tổng kết và viết báo cáo: Hoàn thiện hồ sơ tài liệu bài tập lớn.'))

content.append(p_heading('1.5. Phương pháp tiếp cận', 1))
content.append(p_body('1. Phương pháp nghiên cứu tài liệu: Thu thập, đọc và nghiên cứu tài liệu chính thức từ Microsoft Docs về .NET 8 Web API, các chuẩn bảo mật JWT và kiến trúc RESTful.'))
content.append(p_body('2. Phương pháp tiếp cận hướng dịch vụ (SOA): Tách biệt triệt để phần cung cấp dịch vụ dữ liệu (Backend Web API) với phần tiêu thụ dịch vụ (Frontend Client).'))
content.append(p_body('3. Phương pháp phân tích - thiết kế hướng đối tượng: Áp dụng các nguyên lý SOLID, thiết kế Interface kết hợp Dependency Injection.'))
content.append(p_body('4. Phương pháp thực nghiệm (Iterative Development): Vừa lập trình vừa kiểm thử liên tục bằng Swagger và Postman để kịp thời phát hiện và tối ưu hóa hệ thống.'))

full_doc_xml = doc_xml_template.format(body_content='\n'.join(content))

src_docx = r'c:\Users\Nguye\source\repos\btl_CuaHangNhuongQuyen\key.docx'
out_docx = r'c:\Users\Nguye\source\repos\btl_CuaHangNhuongQuyen\Bao Cao BTL_Chuong1.docx'

with zipfile.ZipFile(src_docx, 'r') as zin, zipfile.ZipFile(out_docx, 'w', zipfile.ZIP_DEFLATED) as zout:
    for item in zin.infolist():
        if item.filename == 'word/document.xml':
            zout.writestr(item.filename, full_doc_xml.encode('utf-8'))
        else:
            zout.writestr(item.filename, zin.read(item.filename))

print(f'SUCCESS! Created {out_docx} with size {os.path.getsize(out_docx)} bytes')
