using Ado.NetWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace Ado.NetWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IConfiguration config;
        public StudentsController(IConfiguration config) 
        {
            this.config = config;
        }
        [HttpGet]
        [Route("GetAllStudent")]
        public List<StudentModel> GetAllStudent()
        {
            List<StudentModel> lst = new List<StudentModel>();
            SqlConnection con = new SqlConnection(config.GetConnectionString("Addconnection"));
            SqlCommand cmd = new SqlCommand("SELECT * FROM Studenttable", con);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                StudentModel obj = new StudentModel();
                obj.Stid = int.Parse(dt.Rows[i][0].ToString());
                obj.stdname = dt.Rows[i][1].ToString();
                obj.stdemail = dt.Rows[i][2].ToString();
                obj.stdpassword = dt.Rows[i][3].ToString();
                lst.Add(obj);
            }
            return lst;
        }

        [HttpPost]
        [Route("SaveStudent")]
        public string SaveStudent(StudentModel model)
        {
            SqlConnection con = new SqlConnection(config.GetConnectionString("Addconnection"));
            SqlCommand cmd = new SqlCommand("INSERT INTO Studenttable VALUES ('"+model.stdname+"','"+model.stdemail+"','"+model.stdpassword+"')", con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            return "Product Save Done";
        }
       
        [HttpGet]
        [Route("GetStudentByID/{id}")]
        public IActionResult GetStudentByID(int id)
        {
            SqlConnection con = new SqlConnection(config.GetConnectionString("Addconnection"));
            con.Open();

            string selectQuery = "SELECT * FROM Studenttable WHERE StdId = '"+id+"'"; // Assuming 'StdId' is the primary key column name

            SqlCommand cmd = new SqlCommand(selectQuery, con);
           
            SqlDataReader rd = cmd.ExecuteReader();

            if (rd.Read())
            {
                StudentModel student = new StudentModel
                {
                    Stid = (int)rd["StdId"],
                    stdname = (string)rd["stdname"],
                    stdemail = (string)rd["stdemail"],
                    stdpassword = (string)rd["stdpassword"]
                };

                con.Close();

                return Ok(student);
            }
            else
            {
                con.Close();
                return NotFound("Student Not Found");
            }
        }

        [HttpDelete]
        [Route("DeleteStudent/{id}")]
        public IActionResult DeleteStudent(int id)
        {
            SqlConnection con = new SqlConnection(config.GetConnectionString("Addconnection"));
            con.Open();
            SqlCommand cmd = new SqlCommand("delete from Studenttable where StdId='"+id+"'", con);
            int rowaffactted = cmd.ExecuteNonQuery();
            con.Close();
            if(rowaffactted > 0) 
            {
                return Ok("Student Deleted Done");
            }
            else
            {
                return NotFound("Student Not Found");
            }
        }

        [HttpPut]
        [Route("EditStudent/{id}")]
        public IActionResult EditStudent(int id, StudentModel model)
        {
            SqlConnection con = new SqlConnection(config.GetConnectionString("Addconnection"));
            con.Open();
            SqlCommand cmd = new SqlCommand("update Studenttable set StdName='"+model.stdname+ "' , StdEmail='"+model.stdemail+ "', StdPassword='"+model.stdpassword+"' where StdId='" + id + "'", con);
            int rowaffactted = cmd.ExecuteNonQuery();
            con.Close();
            if (rowaffactted > 0)
            {
                return Ok("Student Update Done");
            }
            else
            {
                return NotFound("Student Not Found");
            }
        }

    }
}
