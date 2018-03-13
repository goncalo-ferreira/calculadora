using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Simple_Calculator.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            //preparar os primeiros valores da calculadora
            ViewBag.Visor = "0";
            Session["operadorAnterior"] = "";
            Session["limpar"] = true;
            return View();
        }

        //POST: Home
        [HttpPost]
        public ActionResult Index(string bt, string visor)
        {
            //determinar a acao a executar
            switch (bt)
            {
                case "0":
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                    //recupera o resultado da decisao sobre a limpeza do visor
                    bool limpa = (bool)Session["limpar"];
                    if (limpa || visor.Equals("0") ) visor = bt;
                    else visor += bt;
                    //marca o visor para continuar escrita de operando
                    Session["limpar"] = false;
                    break;
                case "+/-":
                    visor = Convert.ToDouble(visor) * (-1)+"";
                    break;
                case ",":
                    if (!visor.Contains(",")) visor += ",";
                    break;
                case "+":
                case "-":
                case "x":
                case ":":
                case "=":
                    //se nao e a 1a vez que se pressiona um operador
                    if (Session["operadorAnterior"].ToString().Equals(""))
                    {
                        //preservar o valor do VISOR
                        //usando variaveis de sessao
                        Session["primeiroOperando"] = visor;
                        //guardar o valor do operador
                        Session["operadorAnterior"] = bt;
                        //preparar o visor para uma nova introducao
                        Session["limpar"] = true;
                    }
                    else
                    {
                        //agora e que se vai fazer a 'conta'
                        //obter os operandos
                        double primeiroOperando = Convert.ToDouble(Session["primeiroOperando"].ToString());
                        double segundoOperando = Convert.ToDouble(visor);
                        //escolher a operacao a fazer com o operador anterior
                        switch (Session["operadorAnterior"].ToString())
                        {
                            case "+":
                                visor = primeiroOperando + segundoOperando + "";
                                break;
                            case "-":
                                visor = primeiroOperando - segundoOperando + "";
                                break;
                            case "x":
                                visor = primeiroOperando * segundoOperando + "";
                                break;
                            case ":":
                                visor = primeiroOperando / segundoOperando + "";
                                break;
                        } //switch (string)Session["operador"]


                        //preservar os valores fornecidos para operacoes futuras
                        if (bt.Equals("=")) Session["operadorAnterior"] = "";
                        else Session["operadorAnterior"] = bt;

                        Session["primeiroOperando"] = visor;
                        //marcar visor para limpeza
                        Session["limpar"] = true;
                    }
                    break;
                   
                case "C":
                    //limpar a calculadora
                    //isto e, fazer um reset total
                    visor = "0";
                    Session["operadorAnterior"] = "";
                    Session["limpar"] = true;
                    break;
            }

            //enviar o resultado para a View
            ViewBag.Visor = visor;
            return View();
        }   
    }
}