using System;
using APIBoletim.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIBoletim.Domains;
using APIBoletim.Context;
using System.Data.SqlClient;

namespace APIBoletim.Repositories
{
    public class AlunoRepository : IAluno
    {

        // Chamar classe de conecção do banco
        BoletimContext conexao = new BoletimContext();

        // Chamar o objeto que recebe e executa os comandos do banco
        SqlCommand cmd = new SqlCommand();

        public Aluno Alterar(int id, Aluno a)
        {
            cmd.Connection = conexao.Conectar();

            cmd.CommandText = "UPDATE Aluno SET " +
                "Nome = @nome " +
                "Ra = @ra " +
                "Idade = @idade  WHERE IdAluno = @id";
            cmd.Parameters.AddWithValue("@nome", a.Nome);
            cmd.Parameters.AddWithValue("@ra", a.RA);
            cmd.Parameters.AddWithValue("@idade", a.Idade);

            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();

            conexao.Desconectar();

            return a;
        }

        public Aluno BuscarPorId(int Id)
        {
            cmd.Connection = conexao.Conectar();

            cmd.CommandText = "SELECT * FROM Aluno WHERE IdAluno = @id";

            //Atribuir variaveis que vem como argumento
            cmd.Parameters.AddWithValue("@id",Id);

            SqlDataReader dados = cmd.ExecuteReader();

            Aluno a = new Aluno();

            while (dados.Read())
            {
                a.IdAluno   = Convert.ToInt32(dados.GetValue(0));
                a.Nome      = dados.GetValue(1).ToString();
                a.RA        = dados.GetValue(2).ToString();
                a.Idade     = Convert.ToInt32(dados.GetValue(3));
            }

            conexao.Desconectar();

            return a;
        }

        public Aluno Cadastrar(Aluno a)
        {
            cmd.Connection = conexao.Conectar();

            cmd.CommandText = "INSERT INTO Aluno (Nome, RA, Idade)"
                + "VALUES" + "(@nome, @ra, @idade)";
            cmd.Parameters.AddWithValue("@nome", a.Nome);
            cmd.Parameters.AddWithValue("@ra", a.RA);
            cmd.Parameters.AddWithValue("@idade", a.Idade);

            //ExecuteNonQuery é utilizado no PUT, no DELETE e no POST (DMLs)
            cmd.ExecuteNonQuery();

            conexao.Desconectar();

            return a;
        }

        public void Excluir(int id)
        {
            cmd.Connection = conexao.Conectar();

            cmd.CommandText = "DELETE FROM Aluno WHERE IdAluno = @id";
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();

            conexao.Desconectar();
        }

        public List<Aluno> LerTodos()
        {
            // Abrir Conexão
            cmd.Connection = conexao.Conectar();

            //Preparar consulta
            cmd.CommandText = "SELECT * FROM Aluno";

            SqlDataReader dados = cmd.ExecuteReader();

            // Criando lista
            List<Aluno> alunos = new List<Aluno>();
            while (dados.Read())
            {
                alunos.Add(
                    new Aluno()
                    {
                        IdAluno   = Convert.ToInt32(dados.GetValue(0)),
                        Nome      = dados.GetValue(1).ToString(),
                        RA        = dados.GetValue(2).ToString(),
                        Idade     = Convert.ToInt32(dados.GetValue(3))
                    }
                    );
            }


            // Fechar Conexão
            conexao.Desconectar();

            return alunos;
        }  
    }
}
